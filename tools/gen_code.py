import json


def pascal_case_to_snake_case(name):
    return ''.join(['_' + i.lower() if i.isupper() else i for i in name]).lstrip('_')

def gen_code(filename):
    code_1 = []
    code_2 = set()
    code_3 = []
    with open(filename, 'r') as f:
        data = json.load(f)
    tables = data['tables']

    for table, table_config in tables.items():
        table_name = pascal_case_to_snake_case(table)
        code_1.append(f"public const string {table_name} = \"{table}\";")
        fields = table_config['fields']
        for field in fields:
            field_name = field['name']
            code_2.add(f"public const string {field_name} = \"{field_name}\";")
            code_3.append(f"public const string {table_name}__{field_name} = \"[{table}].[{field_name}]\";")

    code = "static class Tbl {" + "\n".join(code_1) + "\n}\n" + "static class Fld {" + "\n".join(code_2) + "\n}" + "static class Field {" + "\n".join(code_3) + "\n}"
    return code

if __name__ == "__main__":
    code = gen_code('database.json')
    with open('Models/managers/field.cs', 'w') as f:
        f.write(code)

    print("Code generated successfully!")
