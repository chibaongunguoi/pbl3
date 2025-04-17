public static class ErrorKey
{
    public const string //
        // general errors
        run_out_of_id = "run_out_of_id",
        // signup errors
        username_taken = "username_taken",
        username_empty = "username_empty",
        name_empty = "name_empty",
        bday_empty = "bday_empty",
        password_empty = "password_empty",
        password_mismatch = "password_mismatch",
        // add course errors
        tch_id_not_exist = "tch_id_not_exist",
        grade_invalid = "grade_must_be_int",
        start_date_missing = "start_date_missing",
        finish_date_missing = "finish_date_missing",
        start_date_invalid = "start_date_invalid",
        finish_date_invalid = "finish_date_invalid",
        subject_invalid = "subject_invalid",
        course_invalid = "course_invalid",
        teacher_invalid = "teacher_invalid";
}

/* EOF */
