import { ReactNode } from "react";

export type AdminPageContentProps = {
  children: ReactNode;
  tabKey: string;
  tabTitle: string;
};

export function AdminPageContent(props: AdminPageContentProps) {
  return <div className="admin-page__content__overview">{props.children}</div>;
}
