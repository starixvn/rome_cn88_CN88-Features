namespace BetList.Details {
    export class BaseDetails {
        public readonly ViewerId = () => $('#viewer-id').val();
        public readonly ViewerLevelId = () => $('#viewer-level-id').val();
        public readonly ColossusBetsPopUpDetailTitle = () => $('#colossusbets-popupdetail-title').val();
    }
}
