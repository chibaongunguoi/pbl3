import random
from datetime import datetime, timedelta
import os
import json


today = datetime.today()

if not os.path.exists("data"):
    os.makedirs("data")


def json_output(file_name, lst):
    with open(f"data/{file_name}.json", "w", encoding="utf-8", newline="") as f:
        f.write(
            "["
            + ",\n".join(
                json.dumps([str(item) for item in obj], ensure_ascii=False)
                for obj in lst
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

num_of_students = 300
num_of_teachers = 300

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
        gender = random.choice(genders)
        surname = random.choice(surnames)
        if gender == "male":
            name_1 = random.choice(male_first_names)
            name_2 = random.choice(male_first_names)
        else:
            name_1 = random.choice(female_first_names)
            name_2 = random.choice(female_first_names)

        name = " ".join([surname, name_1, name_2])
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
        yield f"{result.year}-{result.month}-{result.day}"


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

def create_request(stu_id, semester_id, semester_start_date, semester_finish_date, request_state):
    semester_finish_date = min(semester_finish_date, today)
    joined_day = random_date(semester_start_date, semester_finish_date)
    joined_day_str = f"{joined_day.year}-{joined_day.month}-{joined_day.day}"
    request = (stu_id, semester_id, joined_day_str, request_state)
    requests.append(request)



def create_rating(stu_id, course_start_date, course_finish_date):
    course_finish_date = min(course_finish_date, today)
    rating_date = random_date(course_start_date, course_finish_date)
    rating_date_str = (
        f"{rating_date.year}-{rating_date.month}-{rating_date.day}"
    )
    rating_description = """Lorem Ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."""
    rating_score = random.choices([1,2,3,4,5], weights=[0.05, 0.05, 0.05, 0.05, 0.80], k=1)[0]
    # score = random.choice([1, 2, 3, 4, 5])
    rating = (
        stu_id,
        course_id,
        rating_date_str,
        rating_score,
        rating_description,
    )
    ratings.append(rating)

def create_semester(semester_id, semester_start_date, semester_finish_date, grade, capacity, semester_state):
    global semester_next_id,student_grades

    # format: yyyy-mm-dd

    start_date_str = f"{semester_start_date.year}-{semester_start_date.month}-{semester_start_date.day}"
    finish_date_str = f"{semester_finish_date.year}-{semester_finish_date.month}-{semester_finish_date.day}"

    fee = random.choice(["1250000", "1500000", "1750000", "2000000", "2150000"])
    semester = [
        semester_id,
        course_id,
        start_date_str,
        finish_date_str,
        capacity,
        fee,
        semester_description,
        semester_state
    ]
    return semester

    # choose 10-20 students

        # rating date is around 20 days after finish_date

    # if start_start_date > today:
    #     break


# -----------------------------------------------------------------------------

for stu_id in student_ids:
    username = stu_id
    password = stu_id
    gender, name = next(gender_name_gen)
    tel = next(tel_gen)
    bday = next(student_birthday_gen)
    # addr = next(addr_gen)
    students.append((stu_id, username, password, name, gender, bday))
    grade = random.choice([6, 7, 8, 9, 10, 11, 12])
    student_grades[grade].append(stu_id)

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
        subjects.append((subject_next_id, *tup[:3]))
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
    teachers.append(
        (tch_id, username, password, name, gender, bday, thumbnail, description)
    )

    subjects_ = random.choice(subject_groups)
    sbj_name = random.choice(subjects_[0])

    teacher_subjects = []
    for grade in subjects_[1]:
        random_result = random.random()
        if random_result > 0.5:
            continue
        sbj = sbj_name + " " + str(grade)
        sbj_id = subject_dict[sbj]

        course_id = course_next_id
        semester_description = (
            f"Khóa học {sbj} của giáo viên {name}. "
            + r"Lorem Ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        )

        # create semesters

        course_start_date= datetime.strptime("2023-7-1", "%Y-%m-%d") + timedelta(random.randint(0, 100))
        semester_start_date = None
        semester_finish_date = None
            
        indices = list(range(1, 5))
        last_index = len(indices) - 1
        semester_start_date = course_start_date
        course_semesters = []
        semester_state = None
        for i in indices:
            semester_id = semester_next_id
            semester_next_id += 1
            semester_capacity = random.choice([30, 40, 50, 60])
            semester_finish_date = semester_start_date + timedelta(random.randint(90, 120))
            semester_state = "finished"
            if semester_finish_date > today:
                semester_state = "waiting"

            semester = create_semester(semester_id, semester_start_date, semester_finish_date, grade, semester_capacity, semester_state)
            semesters.append(semester)
            course_semesters.append(semester)
            semester_start_date = semester_finish_date + timedelta(random.randint(30, 60))

            if semester_state == "waiting":
                break

        course_finish_date = semester_finish_date
        course_state = semester_state

        for i, sem in enumerate(course_semesters):
            sem_id = sem[0]
            sem_capacity = sem[4]
            joined_students = []
            for stu_id in student_grades[grade]:
                if random.random() > 0.5:
                    continue

                if len(joined_students) >= sem_capacity:
                    break

                request_state = "joined"
                if i == len(course_semesters) - 1 and course_state == "waiting":
                    request_state = random.choice(["joined", "waiting"])

                create_request(stu_id, sem_id, semester_start_date, semester_finish_date, request_state)


        if course_state == "waiting" and len(semesters) > 0:
            semesters[-1][-1] = "waiting"

        for stu_id in student_grades[grade]:
            if random.random() > 0.5:
                continue
            create_rating(stu_id, course_start_date, course_finish_date)

        course = (course_id, tch_id, sbj_id, f"Khóa học VDC {sbj}", course_state)
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
    ["Student", student_next_id, student_first_id, student_max_id],
    ["Teacher", teacher_next_id, teacher_first_id, teacher_max_id],
    ["Course", course_next_id, 1, 0],
    ["Semester", semester_next_id, 1, 0],
    ["Subject", subject_next_id, 1, 0],
]

json_output("id_counter", id_counters)

# -----------------------------------------------------------------------------
json_output("admin", [(1, "admin", "admin")])

# -----------------------------------------------------------------------------
print("Generated successfully!")
