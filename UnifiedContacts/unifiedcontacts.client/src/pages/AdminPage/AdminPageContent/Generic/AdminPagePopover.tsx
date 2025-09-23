import Modal from "react-bootstrap/esm/Modal";
import { TAdminPagePopoverSettings } from "../../../../types/Types";

export type AdminPagePopoverProps = {
  popoverSettings: TAdminPagePopoverSettings;
  show: boolean;
  onHide: () => void;
};

export function AdminPagePopover(props: AdminPagePopoverProps) {
  return (
    <Modal
      show={props.show}
      onHide={props.onHide}
      backdrop={props.popoverSettings.canClose ? true : "static"}
      keyboard={props.popoverSettings.canClose}
      dialogClassName="modal-fit-content"
    >
      <Modal.Header closeButton={props.popoverSettings.canClose}>
        <Modal.Title>{props.popoverSettings.header}</Modal.Title>
      </Modal.Header>
      <Modal.Body>{props.popoverSettings.body}</Modal.Body>
      <Modal.Footer>{props.popoverSettings.footer}</Modal.Footer>
    </Modal>
  );
}
