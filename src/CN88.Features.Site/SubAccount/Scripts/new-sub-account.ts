/// <reference path="base-sub-account.ts" />

declare var CryptoJS: any;

const encrypt = function (plainText: string, secretKey: string): string {
    const passPhrase = "a5s8d2e9c172";
    const key = CryptoJS.enc.Utf8.parse(passPhrase + secretKey);
    const iv = key;

    const pwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(plainText), key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

    return pwd.toString();
};

class NewSubAccount extends BaseSubAccount {
    constructor($: JQueryStatic, top: Window) {
        super($, top);
    }

    protected cacheElements(): void {
        super.cacheElements();

        this.elements.requiredPasswordError = this.$("#RequiredPasswordError").val();
        this.elements.passwordWrongRuleError = this.$("#PasswordWrongRuleError").val();
        this.elements.confirmPasswordError = this.$("#ConfirmPasswordError").val();
        this.elements.hashKey = this.$("#HashKey").val();
        this.elements.suffix1 = this.$("#suffix1");
        this.elements.suffix2 = this.$("#suffix2");
        this.elements.password = this.$("#password");
        this.elements.confirmPassword = this.$("#confirmPassword");
    }

    protected bindEvents(): void {
        super.bindEvents();

        this.$("#btnSaveAndNew").click((e: JQueryEventObject) => {
            e.preventDefault();
            this.onSubmit(true);
        });
    }

    protected getFormData() {
        const hashKey: string = this.elements.hashKey;
        const password: string = this.elements.password.val();
        const formData = super.getFormData();
        formData.suffix1 = this.elements.suffix1.val();
        formData.suffix2 = this.elements.suffix2.val();
        formData.password = encrypt(password, hashKey);

        return formData;
    }

    protected validateForm(): boolean {
        const isValid: boolean = super.validateForm();

        if (!isValid) {
            return false;
        }

        const passwordValue = this.elements.password.val();

        if (passwordValue.length === 0) {
            this.showError(this.elements.requiredPasswordError);
            this.elements.password.focus();
            return false;
        }

        if (!this.isValidPassword(passwordValue)) {
            this.showError(this.elements.passwordWrongRuleError);
            this.elements.password.focus();
            return false;
        }

        if (passwordValue !== this.elements.confirmPassword.val()) {
            this.showError(this.elements.confirmPasswordError);
            this.elements.confirmPassword.focus();

            return false;
        }

        return true;
    }

    private isValidPassword(password: string) {
        const whiteSpace: RegExp = /^\S*$/;
        const whiteSpaceCase: boolean = password.search(whiteSpace) !== -1;

        const regex1: RegExp = /^(?=.{6,12}$).*\d/i;
        const regex2: RegExp = /^(?=.{6,12}$).*[a-z]/;
        const regex3: RegExp = /^(?=.{6,12}$).*[A-Z]/;
        const regex4: RegExp = /^(?=.{6,12}$).*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]/i;

        const case1: number = password.search(regex1) !== -1 ? 1 : 0;
        const case2: number = password.search(regex2) !== -1 ? 1 : 0;
        const case3: number = password.search(regex3) !== -1 ? 1 : 0;
        const case4: number = password.search(regex4) !== -1 ? 1 : 0;

        return (case1 + case2 + case3 + case4 >= 2) && whiteSpaceCase;
    }
}

$(() => {
    new NewSubAccount($, top).init();
});