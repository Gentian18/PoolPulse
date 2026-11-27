(() => {
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
