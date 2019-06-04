namespace Common {
    export class PopupDetails {
        private static instance: PopupDetails;
        private topWindow: Window;

        public static Instance(topWindow: Window): PopupDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PopupDetails() : this.instance;
            this.instance.topWindow = topWindow;

            return this.instance;
        }

        public Show = (options: any) => {
            this.topWindow["popupManager"].init($, this.topWindow);
            this.topWindow["popupManager"].open(options);
        }
    }
}