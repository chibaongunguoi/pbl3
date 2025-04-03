using Microsoft.Data.SqlClient;

class Test2
{
    // ------------------------------------------------------------------------
    public static void test() => test3();

    // ------------------------------------------------------------------------
    public static void test2()
    {
        List<Teacher> teachers = find_by_id("2007");
        foreach (Teacher teacher in teachers)
        {
            Console.WriteLine(teacher.ToString());
        }
    }

    // ------------------------------------------------------------------------
    public static void test3() { }

    // ------------------------------------------------------------------------
    public static List<Teacher> find_by_id(string id_)
    {
        List<Teacher> teachers = new();
        if (!RegexPatterns.match(id_, RegexPatterns.numeric))
        {
            return teachers;
        }

        int id = int.Parse(id_);
        Query q = new(Table.teacher);
        q.where_(Field.teacher__id, id);
        teachers = Database.exec_list(conn => q.select<Teacher>(conn));
        return teachers;
    }

    // ------------------------------------------------------------------------
    public static List<BriefTeacherCard> demo2(SqlConnection conn)
    {
        List<BriefTeacherCard> cards = new();
        void func(SqlDataReader reader)
        {
            int pos = 0;
            Teacher teacher = DataReader.get_data_obj<Teacher>(reader, ref pos);
            BriefTeacherCard card = new()
            {
                id = teacher.id,
                name = teacher.name,
                gender = IoUtils.conv(teacher.gender),
                bday = IoUtils.conv(teacher.bday),
                description = teacher.description,
            };

            cards.Add(card);
        }
        Query q = new(Table.teacher);
        q.select(conn, func);
        return cards;
    }

    public static List<BriefCourseCard> get_brief_course_cards(SqlConnection conn)
    {
        List<BriefCourseCard> cards = new();

        void get_card(SqlDataReader reader)
        {
            int pos = 0;
            int course_id = DataReader.get_int(reader, pos++);
            int semester_id = DataReader.get_int(reader, pos++);

            Query q_2 = new(Table.request);
            q_2.where_(Field.request__semester_id, semester_id);
            q_2.where_(Field.request__state, (int)InfoRequestState.JOINED);

            List<Request> requests = Database.exec_list(conn => q_2.select<Request>(conn));
            int num_participants = requests.Count;

            Query q_3 = new(Table.rating);
            q_3.where_(Field.rating__course_id, course_id);
            List<Rating> ratings = Database.exec_list(conn => q_3.select<Rating>(conn));
            int num_ratings = ratings.Count;
            int total_stars = 0;
            foreach (Rating rating in ratings)
            {
                total_stars += rating.stars;
            }
            double avg_stars = num_ratings == 0 ? 0 : (double)total_stars / num_ratings;

            var course_name = DataReader.get_string(reader, pos++);
            var tch_name = DataReader.get_string(reader, pos++);
            var subject = DataReader.get_string(reader, pos++);
            var grade = DataReader.get_int(reader, pos++);
            var start_date = DataReader.get_data_obj<InfoDate>(reader, ref pos);
            var finish_date = DataReader.get_data_obj<InfoDate>(reader, ref pos);
            var capacity = DataReader.get_int(reader, pos++);
            var fee = DataReader.get_int(reader, pos++);

            BriefCourseCard card = new()
            {
                course_name = course_name,
                tch_name = tch_name,
                subject = subject,
                grade = grade,
                dates = $"{IoUtils.conv(start_date)} - {IoUtils.conv(finish_date)}",
                participants = $"{num_participants}/{capacity}",
                avg_rating = $"{Math.Round(avg_stars, 1)}",
                num_ratings = num_ratings,
                fee = IoUtils.conv_fee(fee),
            };
            cards.Add(card);
        }

        Query q = new Query(Table.course);
        q.join(Field.subject__id, Field.course__sbj_id);
        q.join(Field.teacher__id, Field.course__tch_id);
        q.join(Field.semester__course_id, Field.course__id);
        q.where_(
            Field.semester__state,
            [(int)InfoSemesterState.WAITING, (int)InfoSemesterState.STARTED]
        );
        q.output(Field.course__id);
        q.output(Field.semester__id);
        q.output(Field.course__name);
        q.output(Field.teacher__name);
        q.output(Field.subject__name);
        q.output(Field.subject__grade);
        q.output(Field.semester__start_date);
        q.output(Field.semester__finish_date);
        q.output(Field.semester__capacity);
        q.output(Field.semester__fee);
        q.select(conn, get_card);
        return cards;
    }

    // ------------------------------------------------------------------------
}

/* EOF */
