import random
from datetime import datetime, timedelta


def csv_output(file_name, lst):
    with open(f"data/{file_name}.csv", "w", encoding="utf-8") as f:
        for data in lst:
            f.write(",".join(map(str, data)) + "\n")


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
ids = list(range(3001, 3020))
hours = list(range(0, 24))
minutes = list(range(0, 60))

student_ids = list(range(1001, 1020))
teacher_ids = list(range(2001, 2020))

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


demo_users = []
students = []
teachers = []

gender_name_gen = generate_gender_name()
hour_minute_gen = generate_hour_minute()
tel_gen = generate_tel()
student_birthday_gen = generate_day(STUDENT_BIRTHDAY_START, STUDENT_BIRTHDAY_END)
teacher_birthday_gen = generate_day(TEACHER_BIRTHDAY_START, TEACHER_BIRTHDAY_END)
addr_gen = generate_addr()

for id in ids:
    username = id
    password = id
    gender, name = next(generate_gender_name())
    tel = next(tel_gen)
    bday = next(teacher_birthday_gen)
    addr = next(addr_gen)
    working_time = next(hour_minute_gen)
    demo_users.append(
        (
            id,
            username,
            password,
            name,
            gender,
            addr,
            tel,
            bday,
            working_time,
        )
    )

for id in student_ids:
    username = id
    password = id
    gender, name = next(gender_name_gen)
    tel = next(tel_gen)
    bday = next(student_birthday_gen)
    addr = next(addr_gen)
    students.append(
        (
            id,
            username,
            password,
            name,
            gender,
            addr,
            tel,
            bday,
        )
    )

for id in teacher_ids:
    username = id
    password = id
    gender, name = next(gender_name_gen)
    tel = next(tel_gen)
    bday = next(teacher_birthday_gen)
    addr = next(addr_gen)
    teachers.append(
        (
            id,
            username,
            password,
            name,
            gender,
            addr,
            tel,
            bday,
        )
    )


csv_output("demo_user", demo_users)
csv_output("student", students)
csv_output("teacher", teachers)

print("Generated successfully!")
