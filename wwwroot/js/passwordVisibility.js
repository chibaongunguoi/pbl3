document.addEventListener('DOMContentLoaded', function() {
    function initializePasswordFields() {
        const eyeIconWrappers = document.querySelectorAll('.eye-icon-wrapper');
        eyeIconWrappers.forEach(function(wrapper) {
            const passwordInput = wrapper.closest('.pt-add-input-wrapper, .auth-input-wrapper').querySelector('input[type="password"]');
            const eyeOpen = wrapper.querySelector('.eye-open');
            const eyeClose = wrapper.querySelector('.eye-close');
            
            if (eyeOpen && eyeClose && passwordInput) {
                // Hide eye-open icon by default
                eyeOpen.style.display = 'none';
                eyeClose.style.display = 'block';
                
                wrapper.addEventListener('click', function() {
                    if (passwordInput.type === 'password') {
                        passwordInput.type = 'text';
                        eyeOpen.style.display = 'block';
                        eyeClose.style.display = 'none';
                    } else {
                        passwordInput.type = 'password';
                        eyeOpen.style.display = 'none';
                        eyeClose.style.display = 'block';
                    }
                });
            }
        });
    }

    // Initialize on page load
    initializePasswordFields();

    // Re-initialize when content changes (for dynamic forms)
    const observer = new MutationObserver(function(mutations) {
        mutations.forEach(function(mutation) {
            if (mutation.addedNodes.length) {
                initializePasswordFields();
            }
        });
    });

    observer.observe(document.body, {
        childList: true,
        subtree: true
    });
});
