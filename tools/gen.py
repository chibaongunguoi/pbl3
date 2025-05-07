import random
from datetime import datetime, timedelta
from dataclasses import dataclass, fields
import os
import json
from datetime import datetime,date
from typing import Any

START_DATE = "2023-7-1"
COURSE_RATE = 0.1
REQUEST_RATE = 0.3
RATING_RATE = 0.7

SEM_RANGE_START = 3
SEM_RANGE_END = 5

SEMESTER_CAPACITIES = [i*5 for i in range(3, 13)]

DURATION_RANGE_START = 150
DURATION_RANGE_END = 240
LATEST_START_DATE = 400

num_of_students = 900
num_of_teachers = 300


class nstr(str):
    pass

def conv(instance: Any) -> str:
    value_list = []
    for field in fields(instance):
        field_name = field.name
        field_value = getattr(instance, field_name)
        if field_value is None:
            value_list.append("NULL")
        elif isinstance(field_value, nstr):
            value_list.append(f"N'{field_value}'")
        elif isinstance(field_value, str):
            value_list.append(f"'{field_value}'")
        elif isinstance(field_value, datetime):
            value_list.append(f"'{field_value.year}-{field_value.month:02d}-{field_value.day:02d} {field_value.hour:02d}:{field_value.minute:02d}'")
        elif isinstance(field_value, date):
            value_list.append(f"'{field_value.year}-{field_value.month:02d}-{field_value.day:02d}'")
        elif isinstance(field_value, bool):
            value_list.append(str(field_value).upper())
        else:
            value_list.append(str(field_value))
    
    result =  ",".join(value_list)
    return result

@dataclass
class IdCounter:
    name: str
    count:int
    min_id : int
    max_id: int

@dataclass
class Account:
    id: int
    username: str
    password: str

@dataclass
class Admin(Account):
    pass

@dataclass
class User(Account):
    name: nstr
    gender: str
    bday: date
    tel: str

@dataclass
class Course:
    id: int
    tch_id : int
    sbj_id : int
    name: nstr
    status: str

@dataclass
class Rating:
    stu_id: int
    semester_id: int
    timestamp: datetime
    stars :int
    description: nstr

@dataclass
class Request:
    stu_id: int
    semester_id: int
    timestamp: datetime
    status: str

@dataclass
class Semester:
    id: int
    course_id: int
    start_date: date
    finish_date: date
    capacity: int
    fee: int
    description: nstr
    status: str

@dataclass
class Student(User):
    pass


@dataclass
class Subject:
    id: int
    name :nstr
    grade: int

@dataclass
class Teacher(User):
    thumbnail: str
    description: nstr

today = datetime.today()

if not os.path.exists("data"):
    os.makedirs("data")

with open("tools/ratings.json", 'r',encoding="utf-8" ) as f:
    comments = json.load(f)

def json_output(file_name, lst):
    with open(f"data/{file_name}.json", "w", encoding="utf-8", newline="") as f:
        f.write(
            "["
            + ",\n".join(
                json.dumps(conv(obj), ensure_ascii=False) for obj in lst
            )
            + "]"
        )


def read_data(filename):
    result = []
    with open(filename, "r", encoding="utf-8") as f:
        for line in f.readlines():
            result.append(line.strip())
    return result


surnames = read_data("tools/name/surname.txt")
street_names = read_data("tools/name/street.txt")
male_first_names = read_data("tools/name/male_first_name.txt")
female_first_names = read_data("tools/name/female_first_name.txt")


genders = ["male", "female"]
hours = list(range(0, 24))
minutes = list(range(0, 60))

grades = list(range(6, 13))

student_first_id = 1001
teacher_first_id = 2001
demo_user_id_prefix = 3001

max_num_of_students = 1000
max_num_of_teachers = 1000

student_next_id = student_first_id + num_of_students
teacher_next_id = teacher_first_id + num_of_teachers
course_next_id = 1
semester_next_id = 1
subject_next_id = 1

student_max_id = student_first_id + max_num_of_students - 1
teacher_max_id = teacher_first_id + max_num_of_teachers - 1

student_ids = list(range(student_first_id, student_next_id))
teacher_ids = list(range(teacher_first_id, teacher_next_id))

STUDENT_BIRTHDAY_START = "2007-1-1"
STUDENT_BIRTHDAY_END = "2013-12-31"

TEACHER_BIRTHDAY_START = "1980-1-1"
TEACHER_BIRTHDAY_END = "2003-12-31"


def generate_gender_name():
    while True:
        result = []
        gender = random.choice(genders)
        surname = random.choice(surnames)
        result.append(surname)

        if random.random() > 0.5:
            surname_2 = random.choice(surnames)
            result.append(surname_2)

        if gender == "male":
            name_1 = random.choice(male_first_names)
            name_2 = random.choice(male_first_names)
        else: # female
            name_1 = random.choice(female_first_names + ["Thị"])
            name_2 = random.choice(female_first_names)

        result.append(name_1)
        result.append(name_2)
        name = " ".join(result)
        yield gender, name


def generate_hour_minute():
    while True:
        hour = random.choice(hours)
        minute = random.choice(minutes)
        yield f"{hour:02d}:{minute:02d}"


def generate_tel():
    generated_numbers = set()
    while True:
        tel = "09"
        for _ in range(8):
            tel += str(random.randint(0, 9))

        if tel in generated_numbers:
            continue

        generated_numbers.add(tel)
        yield tel


def generate_day(start_date, end_date):
    while True:
        start = datetime.strptime(start_date, "%Y-%m-%d")
        end = datetime.strptime(end_date, "%Y-%m-%d")
        result = start + timedelta(
            seconds=random.randint(0, int((end - start).total_seconds()))
        )
        yield result


def generate_addr():
    generated_addr = set()
    while True:
        num = random.randint(1, 500)
        street_name = random.choice(street_names)
        addr = f"{num} {street_name}"
        if addr in generated_addr:
            continue
        generated_addr.add(addr)
        yield addr


gender_name_gen = generate_gender_name()
hour_minute_gen = generate_hour_minute()
tel_gen = generate_tel()
student_birthday_gen = generate_day(STUDENT_BIRTHDAY_START, STUDENT_BIRTHDAY_END)
teacher_birthday_gen = generate_day(TEACHER_BIRTHDAY_START, TEACHER_BIRTHDAY_END)
addr_gen = generate_addr()

def random_date(start_date, end_date):
    delta = end_date - start_date
    random_seconds = random.random() * delta.total_seconds()
    return (start_date + timedelta(seconds=random_seconds)).date()

# -----------------------------------------------------------------------------
students = []
teachers = []
student_grades :dict[int, list] = {grade: [] for grade in grades}

# -----------------------------------------------------------------------------

def create_request(stu_id, semester_id, semester_start_date, semester_finish_date, request_status):
    semester_finish_date = min(semester_finish_date, today)
    request_time = random.randint(0, 24 * 60 * 60 - 1)
    semester_start_date =semester_start_date - timedelta(days=random.randint(0, 30))
    middle_date = semester_start_date + (semester_finish_date - semester_start_date) / 2
    joined_day = random_date(semester_start_date, middle_date)
    joined_date = joined_day + timedelta(seconds=request_time)
    request = Request(stu_id, semester_id, joined_date, request_status)
    requests.append(request)
    return request



def create_rating(stu_id, sem_id, the_request_time, course_finish_date):
    random_days = random.randint(0, 5)
    end_date = course_finish_date + timedelta(days=random_days)
    rating_date = min(today, end_date)
    rating_datetime = rating_date + timedelta(seconds=random.randint(0, 24 * 60 * 60 - 1))
    rating_score = random.choices([1,2,3,4,5], weights=[0.05, 0.05, 0.05, 0.05, 0.80], k=1)[0]
    rating_description = random.choice(comments[f"{rating_score}"])
    # score = random.choice([1, 2, 3, 4, 5])
    rating = Rating(
        stu_id,
        sem_id,
        rating_datetime,
        rating_score,
        nstr(rating_description),
    )
    ratings.append(rating)

def create_semester(semester_id, semester_start_date, semester_finish_date, grade, capacity, semester_status):
    global semester_next_id,student_grades

    # format: yyyy-mm-dd

    fee = random.choice([1250000, 1500000, 1750000, 2000000, 2150000])
    semester = Semester(
        semester_id,
        course_id,
        semester_start_date,
        semester_finish_date,
        capacity,
        fee,
        nstr(semester_description),
        semester_status
    )
    return semester

    # choose 10-20 students

        # rating date is around 20 days after finish_date

    # if start_start_date > today:
    #     break


# -----------------------------------------------------------------------------

studentSemesterLimit = {}

for stu_id in student_ids:
    username = stu_id
    password = stu_id
    gender, name = next(gender_name_gen)
    tel = next(tel_gen)
    bday = next(student_birthday_gen)
    # addr = next(addr_gen)
    student = Student(stu_id, str(username), str(password), nstr(name), gender, bday, tel)
    students.append(student)
    grade = random.choice([6, 7, 8, 9, 10, 11, 12])
    student_grades[grade].append(stu_id)
    studentSemesterLimit[stu_id] = random.randint(4, 10)

json_output("student", students)

subject_groups = [
    [["Toán", "Ngữ Văn", "Tiếng Anh", "Vật lí"], [6, 7, 8, 9]],
    [["Hóa học"], [8, 9]],
    [["Toán", "Ngữ Văn", "Tiếng Anh", "Vật lí", "Hóa học", "Sinh học"], [10, 11, 12]],
]


# -----------------------------------------------------------------------------
subjects = []
subject_dict = dict()
subject_infos = dict()

with open("tools/subject.csv", encoding="utf-8") as f:
    for line in f.readlines():
        tup = line.strip().split(",")
        subject = Subject(subject_next_id, nstr(tup[0]), int(tup[1]))
        subjects.append(subject)
        sbj_name = tup[0]
        grade = tup[1]
        subject_dict[sbj_name + " " + grade] = subject_next_id
        subject_infos[subject_next_id] = tup
        subject_next_id += 1

json_output("subject", subjects)

# -----------------------------------------------------------------------------
courses = []
semesters = []
requests = []
ratings = []

for tch_id in teacher_ids:
    username = tch_id
    password = tch_id
    gender, name = next(gender_name_gen)
    tel = next(tel_gen)
    bday = next(teacher_birthday_gen)
    addr = next(addr_gen)
    thumbnail = r"./images/thumbnail/thumbnail.jpg"
    description = f"""Liên hệ gia sư {name} qua số điện thoại {tel} hoặc địa chỉ {addr}.
- Giúp bạn out trình mấy con gà không đi học thêm
- Không sợ out meta, top 1 tri thức hệ toán
- VDC không còn khó, đại học Vinh chỉ còn là cái tên
- Giúp bạn out trình mấy con gà không đi học thêm
- Không sợ out meta, top 1 tri thức hệ toán
- VDC không còn khó, đại học Vinh chỉ còn là cái tên
- Giúp bạn out trình mấy con gà không đi học thêm
- Không sợ out meta, top 1 tri thức hệ toán
- VDC không còn khó, đại học Vinh chỉ còn là cái tên
- Giúp bạn out trình mấy con gà không đi học thêm
- Không sợ out meta, top 1 tri thức hệ toán
- VDC không còn khó, đại học Vinh chỉ còn là cái tên
- Giúp bạn out trình mấy con gà không đi học thêm
- Không sợ out meta, top 1 tri thức hệ toán
- VDC không còn khó, đại học Vinh chỉ còn là cái tên
- Giúp bạn out trình mấy con gà không đi học thêm
- Không sợ out meta, top 1 tri thức hệ toán
- VDC không còn khó, đại học Vinh chỉ còn là cái tên
"""
    teacher = Teacher(tch_id, str(username), str(password), nstr(name), gender, bday,tel, thumbnail, nstr(description))
    teachers.append(teacher)

    subjects_ = random.choice(subject_groups)
    sbj_name = random.choice(subjects_[0])

    teacher_subjects = []
    for grade in subjects_[1]:
        random_result = random.random()
        if random_result > COURSE_RATE:
            continue
        sbj = sbj_name + " " + str(grade)
        sbj_id = subject_dict[sbj]

        course_id = course_next_id
        semester_description = (
            f"Khóa học {sbj} của giáo viên {name}. "
            + r"Lorem Ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        )

        # create semesters

        course_start_date= datetime.strptime(START_DATE, "%Y-%m-%d") + timedelta(random.randint(0, LATEST_START_DATE))
        semester_start_date = None
        semester_finish_date = None
            
        numSemesters = random.randint(SEM_RANGE_START, SEM_RANGE_END)
        indices = list(range(1, numSemesters + 1))
        last_index = len(indices) - 1
        semester_start_date = course_start_date
        course_semesters = []
        semester_status = None
        for i in indices:
            semester_id = semester_next_id
            semester_next_id += 1
            semester_capacity = random.choice(SEMESTER_CAPACITIES)
            semester_finish_date = semester_start_date + timedelta(random.randint(DURATION_RANGE_START, DURATION_RANGE_END))
            semester_status = "waiting"
            if semester_start_date <= today:
                semester_status = "started"
            if semester_finish_date < today:
                semester_status = "finished"

            semester = create_semester(semester_id, semester_start_date, semester_finish_date, grade, semester_capacity, semester_status)
            semesters.append(semester)
            course_semesters.append(semester)
            semester_start_date = semester_finish_date + timedelta(random.randint(30, 60))

            if semester_status == "waiting":
                break

        course_finish_date = semester_finish_date
        course_status = semester_status

        for i, sem in enumerate(course_semesters):
            sem_id = sem.id
            sem_capacity = sem.capacity
            sem_start_date = sem.start_date
            sem_finish_date = sem.finish_date
            sem_status = sem.status
            joined_students = []
            for stu_id in student_grades[grade]:
                if len(joined_students) >= sem_capacity:
                    break

                if studentSemesterLimit[stu_id] == 0:
                    continue                

                if random.random() > REQUEST_RATE:
                    continue

                studentSemesterLimit[stu_id] -= 1

                joined_students.append(stu_id)
                request_status = "joined"
                if i == len(course_semesters) - 1 and course_status == "waiting":
                    request_status = random.choice(["joined", "waiting"])

                request_date_ = None
                the_request = create_request(stu_id, sem_id, sem_start_date, sem_finish_date, request_status)

                the_request_time = the_request.timestamp
                the_request_status = the_request.status

                if random.random() > RATING_RATE or the_request_status == "waiting" or sem_status != "finished":
                    continue
                create_rating(stu_id, sem_id, the_request_time, sem_finish_date)


        if course_status == "waiting" and len(semesters) > 0:
            semesters[-1].status = "waiting"

        course = Course(course_id, tch_id, sbj_id, nstr(f"Khóa học VDC {sbj}"), str(course_status))
        courses.append(course)
        course_next_id += 1


json_output("teacher", teachers)
json_output("course", courses)
json_output("semester", semesters)
json_output("request", requests)
json_output("rating", ratings)

# -----------------------------------------------------------------------------
# intervals = [
#     ("7:30", "9:00"),
#     ("9:30", "11:00"),
#     ("13:30", "15:00"),
#     ("15:30", "17:00"),
#     ("17:30", "19:00"),
#     ("19:30", "21:00"),
# ]
# days = range(7)
# day_intervals = [(day, start, end) for day in days for start, end in intervals]
# teacher_schedules = []
# schedules = []
#
# for pos in range(len(day_intervals)):
#     schedules.append((pos + 1, *day_intervals[pos]))
#
# for tch_id in teacher_ids:
#     for pos in range(1, 7 * len(intervals) + 1):
#         if random.random() > 0.25:
#             continue
#
#         teacher_schedules.append((tch_id, pos))
#
# csv_output("schedule", schedules)
# csv_output("teacher_schedule", teacher_schedules)

# -----------------------------------------------------------------------------

id_counters = [
    IdCounter("Student", student_next_id, student_first_id, student_max_id),
    IdCounter("Teacher", teacher_next_id, teacher_first_id, teacher_max_id),
    IdCounter("Course", course_next_id, 1, 0),
    IdCounter("Semester", semester_next_id, 1, 0),
    IdCounter("Subject", subject_next_id, 1, 0),
]

json_output("id_counter", id_counters)

# -----------------------------------------------------------------------------
json_output("admin", [Admin(1, "admin", "admin")])

# -----------------------------------------------------------------------------
print("Generated successfully!")
