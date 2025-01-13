import os

comment = '''
PBL2 - Dự án lập trình tính toán
Đề tài: Tìm kiếm gia sư cho học viên

Thành viên thực hiện:
    Nguyễn Chí Bảo      23T_DT2
    Nguyễn Quang Đức    23T_KHDL1
    Nguyễn Duy Thành    23T_KHDL1
'''

def format_comment(comment):
    return '\n'.join([f'//{"" if line.strip() == "" else "  " + line}' for line in comment.strip().split('\n')]) + '\n\n'

def process_file(file_path):
    with open(file_path, 'r', encoding='utf-8') as file:
        lines = file.readlines()

    if lines and lines[0].startswith('//'):
        end_comment_index = next((i for i, line in enumerate(lines) if not line.startswith('//')), -1)
        if end_comment_index != -1:
            lines = lines[end_comment_index:]
            while lines and lines[0].strip() == '':
                lines.pop(0)

    new_content = format_comment(comment) + ''.join(lines)

    with open(file_path, 'w', encoding='utf-8') as file:
        file.write(new_content)

def process_directory(directory):
    for root, _, files in os.walk(directory):
        for file in files:
            if file.endswith('.hpp') or file.endswith('.cpp'):
                process_file(os.path.join(root, file))

if __name__ == "__main__":
    process_directory('src')
