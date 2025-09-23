import { TAdminPageMetrics } from "../../../types/Types";
import { AdminpageContentMetrics } from "./AdminPageContentMetrics";

export type AdminPageContentOverviewProps = {
  metrics?: TAdminPageMetrics;
};

export function AdminPageContentOverview(props: AdminPageContentOverviewProps) {
  return (
    <>
      <h1>Overview</h1>
      <div>
        Welcome to Unified Contacts! Be sure to checkout the{" "}
        <a
          href="https://docs.unified-contacts.com/"
          target="_blank"
          rel="noopener noreferrer"
        >
          documentation
        </a>
        .
      </div>
      <h1>Metrics</h1>
      <AdminpageContentMetrics metrics={props.metrics} />
    </>
  );
}
