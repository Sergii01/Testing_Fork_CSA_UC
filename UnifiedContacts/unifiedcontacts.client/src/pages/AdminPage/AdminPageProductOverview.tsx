import { useEffect, useState } from "react";
import Badge from "react-bootstrap/Badge";
import Spinner from "react-bootstrap/esm/Spinner";
import unifiedContactsLogo from "../../assets/images/shape-products-unified-contacts.svg";
import { getAdminControllerGetDependenciesStatus } from "../../services/ApiService";
import { EDependencyStatus } from "../../types/Enums";
import {
  TAdminControllerGetVersionUpdateInfo,
  TAdminGetDependenciesStatus,
} from "../../types/Types";
import OverlayTrigger from "react-bootstrap/OverlayTrigger";
import Tooltip from "react-bootstrap/Tooltip";
import { useAdminPageEnvironmentContext } from "../../providers/AdminPageEnvironmentContextProvider";

export type AdminPageProductOverviewProps = {
  versionUpdateInfo?: TAdminControllerGetVersionUpdateInfo;
  setActiveTabKey: (tabKey: string) => void;
};

export function AdminPageProductOverview(props: AdminPageProductOverviewProps) {
  const [dependenciesStatus, setDependenciesStatus] = useState<
    TAdminGetDependenciesStatus | undefined
  >();
  const environmentContext = useAdminPageEnvironmentContext();

  useEffect(() => {
    getAdminControllerGetDependenciesStatus()
      .then((res) => {
        setDependenciesStatus(res);
      })
      .catch(() => {
        setDependenciesStatus({
          dependencies: [
            {
              displayName: "Admin Page Backend",
              status: EDependencyStatus.ERROR,
            },
          ],
        });
      });
  }, []);

  function getUpdateBatch(
    versionUpdateInfo?: TAdminControllerGetVersionUpdateInfo
  ) {
    if (!versionUpdateInfo || !versionUpdateInfo.updateAvailable) {
      return <></>;
    }
    return (
      <Badge
        bg="danger"
        className="admin-page__productoverview__update-available-batch"
        onClick={() => {
          props.setActiveTabKey("update");
        }}
      >
        Version outdated
      </Badge>
    );
  }

  function getDependencyDetails(details?: TAdminGetDependenciesStatus) {
    if (details) {
      return details.dependencies.map((dependencyStatus) => {
        let badgeStyle: string;
        switch (dependencyStatus.status) {
          case EDependencyStatus.HEALTHY:
            badgeStyle = "success";
            break;
          case EDependencyStatus.WARNING:
            badgeStyle = "warning";
            break;
          case EDependencyStatus.ERROR:
            badgeStyle = "danger";
            break;
          default:
            badgeStyle = "secondary";
        }

        return (
          <p key={dependencyStatus.displayName} className="admin-page__detail">
            <span className="admin-page__detail__property">
              {dependencyStatus.displayName}
            </span>
            {dependencyStatus.statusDescription ? (
              <span className="admin-page__detail__value admin-page__detail__tooltip">
                <OverlayTrigger
                  placement="right"
                  overlay={
                    <Tooltip id={`tooltip-top`}>
                      {dependencyStatus.statusDescription}
                    </Tooltip>
                  }
                >
                  <Badge bg={badgeStyle}>
                    {dependencyStatus.status.replaceAll("_", " ")}
                  </Badge>
                </OverlayTrigger>
              </span>
            ) : (
              <span className="admin-page__detail__value">
                <Badge bg={badgeStyle}>{dependencyStatus.status}</Badge>
              </span>
            )}
          </p>
        );
      });
    } else {
      return (
        <div className="admin-page__detail">
          <span className="admin-page__detail__property">
            Admin Page Backend
          </span>
          <span className="admin-page__detail__value">
            <Spinner animation="border" size="sm" />
          </span>
        </div>
      );
    }
  }

  return (
    <div className="admin-page__productoverview">
      <div className="admin-page__text-center">
        <img
          src={unifiedContactsLogo}
          alt="Unified Contacts Logo"
          draggable={false}
          className="admin-page__productoverview__product-logo"
        />
        <h4 className="admin-page__productoverview__product-name">
          Unified Contacts
        </h4>
      </div>
      <div className="admin-page__detail__text admin-page__productoverview__versiondetails">
        <div className="admin-page__detail">
          <span className="admin-page__detail__property">Version</span>
          <span className="admin-page__detail__value">
            {environmentContext.versionInfo?.version ? (
              <>
                <a
                  style={{ textDecoration: "none" }}
                  href="https://aka.c4a8.net/ucchangelog"
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  {environmentContext.versionInfo?.version}
                </a>
              </>
            ) : (
              <Spinner animation="border" size="sm" />
            )}
          </span>
        </div>
        {getUpdateBatch(props.versionUpdateInfo)}
        <div className="clear-both" />
      </div>
      <div className="admin-page__detail__text admin-page__productoverview__dependencydetails">
        {getDependencyDetails(dependenciesStatus)}
      </div>
      <div className="admin-page__detail__text admin-page__productoverview__copyright">
        Designed and coded in Deggendorf & Offenbach am Main, Germany. <br />
        Copyright © {new Date().getFullYear()}{" "}
        <a href="https://aka.c4a8.net/ucmanufacturerhomepage">glueckkanja AG</a>
        . All rights reserved.
      </div>
    </div>
  );
}
