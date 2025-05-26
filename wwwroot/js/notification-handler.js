// Global notification handler
document.addEventListener('DOMContentLoaded', function() {
    // Function to close all notification dropdowns
    function closeAllNotificationDropdowns() {
        const dropdowns = document.querySelectorAll('.noti-dropdown');
        dropdowns.forEach(dropdown => {
            dropdown.style.display = 'none';
        });
    }
    
    // Ensure notification dropdowns have the highest z-index
    function ensureHighestZIndex(element) {
        // Get all z-indexed elements
        const allElements = document.querySelectorAll('*');
        let highestZIndex = 0;
        
        allElements.forEach(el => {
            const zIndex = parseInt(window.getComputedStyle(el).zIndex);
            if (zIndex && !isNaN(zIndex) && zIndex > highestZIndex) {
                highestZIndex = zIndex;
            }
        });
        
        // Set the notification dropdown to be higher than the highest z-index
        element.style.zIndex = (highestZIndex + 10).toString();
    }
    
    // Apply to all notification dropdowns
    const notificationDropdowns = document.querySelectorAll('.noti-dropdown');
    notificationDropdowns.forEach(dropdown => {
        ensureHighestZIndex(dropdown);
    });
    
    // Close dropdowns when clicking outside
    document.addEventListener('click', function(event) {
        const notificationIcon = event.target.closest('.notification-icon-wrapper');
        const notificationDropdown = event.target.closest('.noti-dropdown');
        
        if (!notificationIcon && !notificationDropdown) {
            closeAllNotificationDropdowns();
        }
    });
    
    // Handle ESC key to close dropdowns
    document.addEventListener('keydown', function(event) {
        if (event.key === 'Escape') {
            closeAllNotificationDropdowns();
        }
    });
    
    // Add event listener for notification icon clicks
    const notificationIcons = document.querySelectorAll('.notification-icon-wrapper');

    notificationIcons.forEach(icon => {
        icon.addEventListener('click', function(event) {
            const dropdown = icon.querySelector('.noti-dropdown');
            if (dropdown) {
                // Check if the dropdown has content
                if (!dropdown.innerHTML.trim()) {
                    console.warn('Dropdown is empty. Please ensure it is populated with content.');
                    return;
                }
                const isVisible = dropdown.style.display === 'block';
                // Close all dropdowns first
                closeAllNotificationDropdowns();
                // Toggle the clicked dropdown
                dropdown.style.display = isVisible ? 'none' : 'block';
            }
        });
    });
});
