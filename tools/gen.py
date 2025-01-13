import random


def csv_output(file_name):
    with open(f"data/{file_name}.csv", "w", encoding="utf-8") as f:
        for data in results:
            f.write(",".join(map(str, data)) + "\n")


surnames = []
male_first_names = []
female_first_names = []

with open("tools/name/surname.txt", "r", encoding="utf-8") as f:
    for line in f.readlines():
        surnames.append(line.strip())

with open("tools/name/male_first_name.txt", "r", encoding="utf-8") as f:
    for line in f.readlines():
        male_first_names.append(line.strip())

with open("tools/name/female_first_name.txt", "r", encoding="utf-8") as f:
    for line in f.readlines():
        female_first_names.append(line.strip())


genders = list(range(0, 2))
ids = list(range(1001, 1021))

results = []

for id in ids:
    name = ""
    gender = random.choice(genders)
    if gender == 0:
        name = (
            random.choice(surnames)
            + " "
            + random.choice(male_first_names)
            + " "
            + random.choice(male_first_names)
        )
    else:
        name = (
            random.choice(surnames)
            + " "
            + random.choice(female_first_names)
            + " "
            + random.choice(female_first_names)
        )
    results.append((id, name))


csv_output("demo_user")
