import {
  Button,
  Input,
  InputOnChangeData,
  InputProps,
} from "@fluentui/react-components";
import { Dismiss24Regular, Search24Regular } from "@fluentui/react-icons";
import { MutableRefObject, useRef, useState } from "react";
import localizedStrings from "../loacalization/localization";

export type ControlBarProps = {
  onSearchTrigger: (query: string) => Promise<void>;
  pendingSearch: boolean;
};

const ControlBar = (props: ControlBarProps) => {
  const inputRef = useRef() as MutableRefObject<HTMLInputElement>;
  const [inputValue, setInputValue] = useState("");

  const onChange: InputProps["onChange"] = (
    _event: React.ChangeEvent<HTMLInputElement>,
    data: InputOnChangeData
  ) => {
    setInputValue(data.value);
  };
  const onKeyDown: InputProps["onKeyDown"] = (
    event: React.KeyboardEvent<HTMLInputElement>
  ) => {
    if (
      event.key === "Enter" &&
      inputRef.current?.value?.trim().length > 0 &&
      !props.pendingSearch
    ) {
      props.onSearchTrigger(inputRef.current?.value.trim());
    }
  };
  const clear = () => {
    setInputValue("");
  };

  return (
    <div className="header__search-box__control-bar">
      <div className="header__search-box__control-bar__search-bar">
        <Input
          ref={inputRef}
          className="header__search-box__control-bar__search-bar__input"
          value={inputValue}
          contentAfter={
            !inputValue || inputValue.trimStart().length <= 0 ? (
              <Search24Regular />
            ) : (
              <Dismiss24Regular onClick={clear} />
            )
          }
          placeholder={localizedStrings.search}
          onChange={onChange}
          onKeyDown={onKeyDown}
        />
      </div>
      <Button
        appearance="primary"
        disabled={
          !inputValue ||
          inputValue.trimStart().length <= 0 ||
          props.pendingSearch
        }
        onClick={() => props.onSearchTrigger(inputRef.current?.value.trim())}
        className="header__search-box__search-button"
      >
        {localizedStrings.search}
      </Button>
    </div>
  );
};

export default ControlBar;
