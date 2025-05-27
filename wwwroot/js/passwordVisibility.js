function initializePasswordFields() {
    const eyeIconWrappers = document.querySelectorAll('.eye-icon-wrapper');
    eyeIconWrappers.forEach(function (wrapper) {
        if (wrapper.dataset.initialized) return;
        wrapper.dataset.initialized = true;
        const passwordInput = wrapper.closest('.pt-add-input-wrapper, .auth-input-wrapper').querySelector('input[type="password"]');
        const eyeOpen = wrapper.querySelector('.eye-open');
        const eyeClose = wrapper.querySelector('.eye-close');

        if (eyeOpen && eyeClose && passwordInput) {
            eyeOpen.style.display = 'none';
            eyeClose.style.display = 'block';

            wrapper.addEventListener('click', function () {
                if (passwordInput.type === 'password') {
                    console.log('Changing password input type to text');
                    passwordInput.type = 'text';
                    eyeOpen.style.display = 'block';
                    eyeClose.style.display = 'none';
                } else {
                    console.log('Changing password input type to password');
                    passwordInput.type = 'password';
                    eyeOpen.style.display = 'none';
                    eyeClose.style.display = 'block';
                }
            });
        }
    });
}

initializePasswordFields();