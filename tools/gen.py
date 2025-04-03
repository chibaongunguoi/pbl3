import random
from datetime import datetime, timedelta
import os
import json

if not os.path.exists("data"):
    os.makedirs("data")


def csv_output(file_name, lst):
    with open(f"data/{file_name}.json", "w", encoding="utf-8", newline="") as f:
        json.dump(
            [[str(item) for item in obj] for obj in lst],
            f,
            ensure_ascii=False,
            indent=4,
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


genders = list(range(0, 2))
hours = list(range(0, 24))
minutes = list(range(0, 60))

student_first_id = 1001
teacher_first_id = 2001
demo_user_id_prefix = 3001

num_of_students = 20
num_of_teachers = 20

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
        if gender == 1:
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

# -----------------------------------------------------------------------------

# demo_users = []
students = []
teachers = []

# for id in demo_user_ids:
#     username = id
#     password = id
#     gender, name = next(generate_gender_name())
#     tel = next(tel_gen)
#     bday = next(teacher_birthday_gen)
#     addr = next(addr_gen)
#     working_time = next(hour_minute_gen)
#     demo_users.append(
#         (id, username, password, name, gender, addr, tel, bday, working_time)
#     )

for stu_id in student_ids:
    username = stu_id
    password = stu_id
    gender, name = next(gender_name_gen)
    tel = next(tel_gen)
    bday = next(student_birthday_gen)
    # addr = next(addr_gen)
    students.append((stu_id, username, password, name, gender, bday))

csv_output("student", students)

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
        subject_dict[sbj_name] = subject_next_id
        subject_infos[subject_next_id] = tup
        subject_next_id += 1

csv_output("subject", subjects)

# -----------------------------------------------------------------------------
courses = []
semesters = []

for tch_id in teacher_ids:
    username = tch_id
    password = tch_id
    gender, name = next(gender_name_gen)
    tel = next(tel_gen)
    bday = next(teacher_birthday_gen)
    addr = next(addr_gen)
    thumbnail = r"./images/thumbnail/thumbnail.jpg"
    description = f"""Liên hệ gia sư {name} qua số điện thoại {tel} hoặc địa chỉ {addr}.,
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
        course_description = (
            f"Khóa học {sbj} của giáo viên {name}. "
            + r"Lorem Ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        )
        course = (course_id, tch_id, sbj_id, f"Khóa học VDC {sbj}", course_description)
        courses.append(course)
        course_next_id += 1

        # create semesters

        for _ in range(random.randint(1, 3)):
            semester_id = semester_next_id
            capacity = random.choice([30, 40, 50, 60])

            # random date between 2024/1/1 and 2024/12/31
            # finish date is between 30 and 90 days after start date
            start_start_date = datetime.strptime("2024-1-1", "%Y-%m-%d")
            end_start_date = datetime.strptime("2024-12-31", "%Y-%m-%d")

            start_date = start_start_date + timedelta(
                random.randint(0, (end_start_date - start_start_date).days)
            )
            finish_date = start_date + timedelta(days=random.randint(30, 90))

            # format: yyyy-mm-dd

            start_date = f"{start_date.year}-{start_date.month}-{start_date.day}"
            finish_date = f"{finish_date.year}-{finish_date.month}-{finish_date.day}"

            fee = subject_infos[sbj_id][3]
            semester = (semester_id, course_id, start_date, finish_date, capacity, fee)
            semesters.append(semester)

            semester_next_id += 1


csv_output("teacher", teachers)
csv_output("course", courses)
csv_output("semester", semesters)

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
    ["student", student_next_id, student_first_id, student_max_id],
    ["teacher", teacher_next_id, teacher_first_id, teacher_max_id],
    ["course", course_next_id, 1, 0],
    ["semester", semester_next_id, 1, 0],
    ["subject", 1, 1, 0],
]

csv_output("id_counter", id_counters)

# -----------------------------------------------------------------------------
csv_output("admin", [(1, "admin", "admin")])
csv_output("rating", [])
csv_output("request", [])

# -----------------------------------------------------------------------------
print("Generated successfully!")
