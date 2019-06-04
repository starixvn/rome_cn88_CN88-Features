import { BaseSubAccount } from "./base-sub-account";

declare var CryptoJS: any;

const encrypt = function (plainText: string, secretKey: string): string {
    const passPhrase = "a5s8d2e9c172";
    let key = CryptoJS.enc.Utf8.parse(passPhrase + secretKey);
    let iv = key;

    let pwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(plainText), key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

    return pwd.toString();
};

class NewSubAccount extends BaseSubAccount {
    constructor($: JQueryStatic, top: any) {
        super($, top);
    }

    protected cacheElements(): void {
        super.cacheElements();
        this.elements.passRule = this.$("#pass-rule");
        this.elements.passRule.tooltip();
        this.elements.requiredPasswordError = this.$("#RequiredPasswordError").val();
        this.elements.passwordWrongRuleError = this.$("#PasswordWrongRuleError").val();
        this.elements.confirmPasswordError = this.$("#ConfirmPasswordError").val();
        this.elements.hashKey = this.$("#HashKey").val();
        this.elements.suffix1 = this.$("#suffix1");
        this.elements.suffix2 = this.$("#suffix2");
        this.elements.password = this.$("#password");
        this.elements.confirmPassword = this.$("#confirmPassword");
    }

    protected getFormData(): any {
        let hashKey: string = this.elements.hashKey;
        let password: string = this.elements.password.val();
        let formData: any = super.getFormData();
        formData.suffix1 = this.elements.suffix1.val();
        formData.suffix2 = this.elements.suffix2.val();
        formData.password = encrypt(password, hashKey);

        return formData;
    }

    protected validateFormData(formData: any): boolean {
        let isValid: boolean = super.validateFormData(formData);

        if (!isValid) {
            return false;
        }

        let passwordValue = this.elements.password.val();

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
        let whiteSpace: RegExp = /^\S*$/;
        let whiteSpaceCase: boolean = password.search(whiteSpace) !== -1;

        let regex1: RegExp = /^(?=.{8,100}$).*\d/i;
        let regex2: RegExp = /^(?=.{8,100}$).*[a-z]/;
        let regex3: RegExp = /^(?=.{8,100}$).*[A-Z]/;
        let regex4: RegExp = /^(?=.{8,100}$).*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]/i;

        let case1: number = password.search(regex1) !== -1 ? 1 : 0;
        let case2: number = password.search(regex2) !== -1 ? 1 : 0;
        let case3: number = password.search(regex3) !== -1 ? 1 : 0;
        let case4: number = password.search(regex4) !== -1 ? 1 : 0;

        return (case1 + case2 + case3 + case4 >= 3) && whiteSpaceCase;
    }
}

$(() => {
    new NewSubAccount($, top).init();
});