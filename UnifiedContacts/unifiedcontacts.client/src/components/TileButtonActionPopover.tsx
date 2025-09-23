import { TILE_ACTIONS } from "../types/Enums";
import {
  triggerChat,
  triggerMail,
  triggerPhoneCall,
  triggerVideoCall,
} from "../services/ButtonActionService";
import { ReactElement, useContext } from "react";
import { MenuItem, MenuList, PopoverSurface } from "@fluentui/react-components";
import { useTeamsContext } from "../providers/TeamsContextProvider";
import { TeamsPageAlertServiceContext } from "../providers/TeamsPageAlertServiceContextProvider";

type TileButtonActionPopoverProps = {
  inputArray: string[];
  functionality: TILE_ACTIONS;
};

export const TileButtonActionPopover = (
  props: TileButtonActionPopoverProps
) => {
  const teamsContext = useTeamsContext();
  const teamsAlertService = useContext(TeamsPageAlertServiceContext);
  const clickMenuItem = (element: string) => {
    switch (props.functionality) {
      case TILE_ACTIONS.MAILTO:
        triggerMail(element, teamsAlertService, teamsContext);
        break;
      case TILE_ACTIONS.PHONECALL:
        triggerPhoneCall(element, teamsAlertService, teamsContext);
        break;
      case TILE_ACTIONS.VIDEOCALL:
        triggerVideoCall(element, teamsAlertService, teamsContext);
        break;
      case TILE_ACTIONS.MESSAGE:
        triggerChat(element, teamsAlertService, teamsContext);
        break;
    }
  };
  const buildMenuListItems = (): ReactElement<typeof MenuItem>[] => {
    const menuItems: ReactElement<typeof MenuItem>[] = [];
    props.inputArray.forEach((element) => {
      menuItems.push(
        <MenuItem onClick={() => clickMenuItem(element)}>{element}</MenuItem>
      );
    });
    return menuItems;
  };

  return (
    <>
      {props.inputArray.length > 1 && (
        <PopoverSurface>
          <MenuList>{buildMenuListItems()}</MenuList>
        </PopoverSurface>
      )}
    </>
  );
};
