import Tabs from "react-bootstrap/Tabs";
import Tab from "react-bootstrap/Tab";
import { ReactElement, useMemo } from "react";
import React from "react";

export type AdminPageContentTabsProps = {
  children?: ReactElement | ReactElement[];
  activeTabKey: string;
  onTabSelect: (tabKey: string) => void;
};

export function AdminPageContentTabs(props: AdminPageContentTabsProps) {
  const typeSafeChildren = useMemo<ReactElement[]>(() => {
    if (!props.children) {
      return [];
    }
    if (Array.isArray(props.children)) {
      return props.children;
    }
    return [props.children];
  }, [props.children]);

  return (
    <div className="admin-page__content">
      <Tabs
        activeKey={props.activeTabKey}
        onSelect={(tabKey) => props.onTabSelect(tabKey || props.activeTabKey)}
        id="uncontrolled-tabs"
      >
        {React.Children.map(typeSafeChildren, (child: ReactElement) => {
          return (
            <Tab
              key={child.props.tabKey}
              eventKey={child.props.tabKey}
              title={child.props.tabTitle}
            >
              {child}
            </Tab>
          );
        })}
      </Tabs>
    </div>
  );
}
