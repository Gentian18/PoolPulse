(() => {
    const slider = document.querySelector('[data-auth-slider]');
    if (slider) {
        const slides = Array.from(slider.querySelectorAll('.auth-slide'));
        const controls = slider.querySelector('[data-slider-controls]');
        const buttons = Array.from(slider.querySelectorAll('.auth-dashes button'));
        const pauseButton = slider.querySelector('[data-slider-pause]');
        const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)');
        let current = 0;
        let paused = false;
        let timer;

        function showSlide(index) {
            current = index;
            slides.forEach((slide, i) => slide.classList.toggle('is-active', i === index));
            buttons.forEach((button, i) => {
                button.classList.toggle('is-active', i === index);
                button.setAttribute('aria-pressed', String(i === index));
            });
        }

        function schedule() {
            window.clearInterval(timer);
            pauseButton.hidden = reducedMotion.matches;
            if (paused || reducedMotion.matches) return;
            timer = window.setInterval(() => {
                if (!document.hidden && !slider.matches(':hover') && !slider.contains(document.activeElement)) {
                    showSlide((current + 1) % slides.length);
                }
            }, 6000);
        }

        buttons.forEach((button, index) => button.addEventListener('click', () => {
            showSlide(index);
            schedule();
        }));
        pauseButton.addEventListener('click', () => {
            paused = !paused;
            pauseButton.textContent = paused ? "Foto's afspelen" : "Foto's pauzeren";
            schedule();
        });
        reducedMotion.addEventListener('change', schedule);
        controls.hidden = false;
        schedule();
    }

    document.querySelectorAll('[data-password-toggle]').forEach((button) => {
        const input = document.getElementById(button.dataset.passwordToggle);
        if (!input) return;
        button.hidden = false;
        button.addEventListener('click', () => {
            const visible = input.type === 'password';
            input.type = visible ? 'text' : 'password';
            button.textContent = visible ? 'Verbergen' : 'Tonen';
            button.setAttribute('aria-label', visible ? 'Wachtwoord verbergen' : 'Wachtwoord tonen');
            button.setAttribute('aria-pressed', String(visible));
        });
    });

    const form = document.querySelector('[data-register-form]');
    if (!form || !window.jQuery || !window.jQuery.fn.validate) return;
    window.jQuery(() => {
    const first = form.querySelector('[data-register-step="1"]');
    const second = form.querySelector('[data-register-step="2"]');
    const next = form.querySelector('[data-register-next]');
    const back = form.querySelector('[data-register-back]');
    const indicators = document.querySelector('.auth-steps');
    const validator = window.jQuery(form).validate();

    function showStep(step) {
        first.hidden = step !== 1;
        second.hidden = step !== 2;
        document.querySelectorAll('[data-step-indicator]').forEach((indicator) => {
            const active = Number(indicator.dataset.stepIndicator) === step;
            indicator.classList.toggle('is-active', active);
            if (active) indicator.setAttribute('aria-current', 'step');
            else indicator.removeAttribute('aria-current');
        });
    }

    next.hidden = false;
    back.hidden = false;
    indicators.hidden = false;
    const firstHasErrors = first.querySelector('.input-validation-error, .field-validation-error');
    const secondHasErrors = second.querySelector('.input-validation-error, .field-validation-error');
    const summaryHasErrors = form.querySelector('.validation-summary-errors');
    showStep(!firstHasErrors && (secondHasErrors || summaryHasErrors) ? 2 : 1);

    next.addEventListener('click', () => {
        let valid = true;
        first.querySelectorAll('input:not([type="hidden"]), select').forEach((input) => {
            if (!validator.element(input)) valid = false;
        });
        if (!valid) {
            first.querySelector('.input-validation-error')?.focus();
            return;
        }
        showStep(2);
        second.querySelector('input')?.focus();
    });
    back.addEventListener('click', () => {
        showStep(1);
        first.querySelector('input')?.focus();
    });
    });
})();
