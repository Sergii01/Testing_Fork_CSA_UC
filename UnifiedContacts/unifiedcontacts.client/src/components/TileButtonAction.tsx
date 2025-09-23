import localizedStrings from "../loacalization/localization";
import { Button, Popover, PopoverTrigger } from "@fluentui/react-components";
import {
  Call24Regular,
  Chat24Regular,
  Video24Regular,
  Mention24Regular,
} from "@fluentui/react-icons";
import { TILE_ACTIONS } from "../types/Enums";
import {
  triggerChat,
  triggerMail,
  triggerPhoneCall,
  triggerVideoCall,
} from "../services/ButtonActionService";
import { TileButtonActionPopover } from "./TileButtonActionPopover";
import { useTeamsContext } from "../providers/TeamsContextProvider";
import { TeamsPageAlertServiceContext } from "../providers/TeamsPageAlertServiceContextProvider";
import { useContext } from "react";

type TileButtonActionProps = {
  imEmailArray: string[];
  emailArray: string[];
  phoneArray: string[];
  isCurrentUser: boolean;
  messageButtonId: string;
  emailButtonId: string;
  phoneButtonId: string;
  videoButtonId: string;
};

export const TileButtonAction = (props: TileButtonActionProps) => {
  const teamsContext = useTeamsContext();
  const teamsAlertService = useContext(TeamsPageAlertServiceContext);
  const onClickMessage = () => {
    if (props.isCurrentUser) {
      teamsAlertService?.setTileActionAlert(localizedStrings.teamsMessageCurrentUserError);
    } else {
      if (props.imEmailArray.length > 0) {
        // If multiple imAddresses are specified we use the first one
        triggerChat(
          props.imEmailArray[0],
          teamsAlertService,
          teamsContext
        );
      } else if (props.emailArray.length > 0) {
        //If no imAddresses are specified we use the first email
        triggerChat(
          props.emailArray[0],
          teamsAlertService,
          teamsContext
        );
      }
    }
  };

  const onClickVideo = () => {
    if (props.isCurrentUser) {
      teamsAlertService?.setTileActionAlert(localizedStrings.teamsCallCurrentUserError);
    } else {
      if (props.imEmailArray.length > 0) {
        // If multiple imAddresses are specified we use the first one
        triggerVideoCall(
          props.imEmailArray[0],
          teamsAlertService,
          teamsContext
        );
      } else if (props.emailArray.length > 0) {
        //If no imAddresses are specified we use the first email
        triggerVideoCall(
          props.emailArray[0],
          teamsAlertService,
          teamsContext
        );
      }
    }
  };

  const onClickPhone = () => {
    if (props.phoneArray.length === 1) {
      triggerPhoneCall(
        props.phoneArray[0],
        teamsAlertService,
        teamsContext
      );
    }
  };

  const onClickMail = () => {
    if (props.emailArray.length === 1) {
      triggerMail(props.emailArray[0], teamsAlertService, teamsContext);
    }
  };

  return (
    <div className="tile__action-bar">
      <Popover>
        <PopoverTrigger>
          <Button
            className="button-group--left"
            id={props.messageButtonId}
            icon={<Chat24Regular />}
            onClick={onClickMessage}
            disabled={
              props.imEmailArray.IsNullOrEmpty() &&
              props.emailArray.IsNullOrEmpty()
            }
          />
        </PopoverTrigger>
        <TileButtonActionPopover
          inputArray={props.imEmailArray}
          functionality={TILE_ACTIONS.MESSAGE}
        />
      </Popover>
      <Popover>
        <PopoverTrigger>
          <Button
            className="button-group--inner"
            id={props.videoButtonId}
            icon={<Video24Regular />}
            onClick={onClickVideo}
            disabled={
              props.imEmailArray.IsNullOrEmpty() &&
              props.emailArray.IsNullOrEmpty()
            }
          />
        </PopoverTrigger>
        <TileButtonActionPopover
          inputArray={props.imEmailArray}
          functionality={TILE_ACTIONS.VIDEOCALL}
        />
      </Popover>
      <Popover>
        <PopoverTrigger>
          <Button
            id={props.phoneButtonId}
            className="button-group--inner"
            icon={<Call24Regular />}
            onClick={onClickPhone}
            disabled={props.phoneArray.IsNullOrEmpty()}
          />
        </PopoverTrigger>
        <TileButtonActionPopover
          inputArray={props.phoneArray}
          functionality={TILE_ACTIONS.PHONECALL}
        />
      </Popover>
      <Popover>
        <PopoverTrigger>
          <Button
            className="button-group--right"
            id={props.emailButtonId}
            icon={<Mention24Regular />}
            onClick={onClickMail}
            disabled={props.emailArray.IsNullOrEmpty()}
          />
        </PopoverTrigger>
        <TileButtonActionPopover
          inputArray={props.emailArray}
          functionality={TILE_ACTIONS.MAILTO}
        />
      </Popover>
    </div>
  );
};
