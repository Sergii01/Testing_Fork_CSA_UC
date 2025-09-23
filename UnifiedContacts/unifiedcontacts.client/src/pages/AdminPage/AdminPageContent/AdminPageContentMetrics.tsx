import { TAdminPageMetric, TAdminPageMetrics } from "../../../types/Types";
import ProgressBar from "react-bootstrap/ProgressBar";
import Card from "react-bootstrap/Card";
import { METRIC_TYPE, PROGRESS_BAR_VARIANT } from "../../../types/Enums";
import { useCallback, useMemo } from "react";

export type AdminpageContentMetricsProps = {
  metrics?: TAdminPageMetrics;
};
const WARNING_LIMIT_QUOTA = 0.8;
const WARNING_LIMIT_DISPLAYNUMBER = 0.8;

const ALERT_LIMIT_QUOTA = 1;
const ALERT_LIMIT_DISPLAYNUMBER = 1;

export function AdminpageContentMetrics(props: AdminpageContentMetricsProps) {
  const calculateClassname = (limtReachedPercent: number): string => {
    let classNameSuffix = "";
    if (limtReachedPercent >= ALERT_LIMIT_DISPLAYNUMBER) {
      classNameSuffix = "admin-page__metrics__display-number--alert";
    } else if (limtReachedPercent > WARNING_LIMIT_DISPLAYNUMBER) {
      classNameSuffix = "admin-page__metrics__display-number--warning";
    }
    return classNameSuffix;
  };

  const calculateLimitReached = (metric: TAdminPageMetric): number => {
    return metric.limit !== null && metric.limit !== undefined
      ? metric.value / metric.limit
      : 0;
  };

  const createDisplayNumber = useCallback((metric: TAdminPageMetric) => {
    const limtReachedPercent: number = calculateLimitReached(metric);
    return (
      <Card
        key={metric.displayName}
        className={
          "admin-page__metrics__display-number__card " +
          calculateClassname(limtReachedPercent)
        }
      >
        <Card.Title
          className={
            "admin-page__metrics__display-number__title " +
            calculateClassname(limtReachedPercent)
          }
        >
          {metric.value}
        </Card.Title>
        <Card.Body>
          <Card.Text
            className={
              "admin-page__metrics__display-number__text " +
              calculateClassname(limtReachedPercent)
            }
          >
            {metric.displayName}
          </Card.Text>
        </Card.Body>
      </Card>
    );
  }, []);

  const createQuota = useCallback((metric: TAdminPageMetric) => {
    const labelString: string =
      metric.limit !== null && metric.limit !== undefined
        ? metric.value + "/" + metric.limit
        : metric.value.toString();
    let variant = PROGRESS_BAR_VARIANT.INFO;
    const limtReachedPercent: number = calculateLimitReached(metric);
    if (limtReachedPercent >= ALERT_LIMIT_QUOTA) {
      variant = PROGRESS_BAR_VARIANT.DANGER;
    } else if (limtReachedPercent > WARNING_LIMIT_QUOTA) {
      variant = PROGRESS_BAR_VARIANT.WARNING;
    }
    return (
      <div
        key={metric.displayName}
        className="admin-page__metrics__quota__ccontainer "
      >
        <ProgressBar
          className="admin-page__metrics__quota__progress-bar"
          variant={variant}
          now={metric.value}
          max={metric.limit}
        />
        <div className="admin-page__metrics__quota__metadata">
          <span>{metric.displayName}</span>
          <span className="admin-page__metrics__quota__labelstring">
            {labelString}
          </span>
        </div>
      </div>
    );
  }, []);

  const displayNumberMap = useMemo(() => {
    return props.metrics?.metrics
      .filter((metric) => metric.$metricType === METRIC_TYPE.DISPLAY_NUMBER)
      .map((metric) => {
        return createDisplayNumber(metric);
      });
  }, [props.metrics?.metrics, createDisplayNumber]);

  const quotaMap = useMemo(() => {
    return props.metrics?.metrics
      .filter((metric) => metric.$metricType === METRIC_TYPE.QUOTA)
      .map((metric) => {
        return createQuota(metric);
      });
  }, [props.metrics?.metrics, createQuota]);

  return (
    <>
      <div className="admin-page__metrics__display-number__container">
        {displayNumberMap}
      </div>
      <div>{quotaMap}</div>
    </>
  );
}
