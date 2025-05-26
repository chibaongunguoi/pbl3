# Notification Feature Testing Guide

## Summary of Implementation

The notification feature has been successfully implemented with the following components:

### 1. Database Structure ✅
- **notification** table added to `database.json`
- Fields: id, stu_id, message, timestamp, is_read
- Data file: `data/notification.json` (initialized as empty array)

### 2. Model Implementation ✅
- **Notification.cs** class with properties and static Add method
- Integrates with existing Query system for database operations

### 3. UI Components ✅
- **Rejection Reason Popup**: `Views/Shared/Popup/_RejectRequestReasonPopup.cshtml`
- **Student Notifications View**: `Views/Student/Notifications.cshtml`
- **Navigation Link**: Added to student layout menu

### 4. API Integration ✅
- **RejectRequest endpoint** in `TeacherManageAPI.cs` supports optional reason parameter
- **AcceptRequest endpoint** creates acceptance notifications
- Both endpoints now create appropriate notifications for students

### 5. Student Controller ✅
- **Notifications action** retrieves and displays student notifications
- **MarkNotificationRead action** marks notifications as read
- Proper authentication and student ID retrieval

### 6. Field Mappings ✅
- All notification field constants defined in `field.cs`
- Proper table name mapping in `Tbl` class

## Test Scenarios

### Scenario 1: Teacher Rejects Student Request with Reason
1. Login as teacher
2. Navigate to "Học viên chờ xác nhận" (Manage Requests)
3. Click "Từ chối" (Reject) button for a student request
4. Modal should open asking for rejection reason
5. Enter reason and submit
6. Notification should be created for the student

### Scenario 2: Teacher Accepts Student Request
1. Login as teacher
2. Navigate to "Học viên chờ xác nhận" (Manage Requests)
3. Click "Đồng ý" (Accept) button for a student request
4. Acceptance notification should be created for the student

### Scenario 3: Student Views Notifications
1. Login as student
2. Click "Thông báo" (Notifications) in navigation menu
3. Should see list of notifications with read/unread status
4. Click "Đánh dấu đã đọc" to mark notifications as read

## Technical Details

### Database Operations
- Uses existing Query class with Insert method supporting SqlConnection parameter
- Notifications stored with timestamp and read status
- Student ID properly retrieved using username-based lookup

### Security
- Student controller actions properly authenticated with [Authorize(Roles = UserRole.Student)]
- Teacher API endpoints authenticated with [Authorize(Roles = UserRole.Teacher)]
- Proper authorization checks ensure students only see their own notifications

### Error Handling
- AJAX error handling in rejection popup
- Proper form validation for required reason field
- Graceful handling of empty notification lists

## Status: READY FOR TESTING ✅

All components have been implemented and integrated. The feature is ready for end-to-end testing in the application.
