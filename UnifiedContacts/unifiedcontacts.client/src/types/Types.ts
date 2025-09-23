import { ReactNode } from "react";
import {
  ALERT_TYPE,
  EDependencyStatus,
  SORT_CATEGORY,
  SORT_TYPE,
  TILE_FORMAT,
  EUnifiedContactsEdition,
  METRIC_TYPE,
} from "./Enums";
import { Slot } from "@fluentui/react-components";

export type TUnifiedContactsSearchResponsePhoneNumbers = {
  home?: string[];
  business?: string[];
  mobile?: string[];
  other?: string[];
};

export type TUnifiedContactsSearchResponseAddress = {
  streetAddress?: string;
  postalCode?: string;
  city?: string;
  country?: string;
  addressAltString?: string;
};

export type TUnifiedContactsSearchResponseAddresses = {
  home?: TUnifiedContactsSearchResponseAddress[];
  business?: TUnifiedContactsSearchResponseAddress[];
  other?: TUnifiedContactsSearchResponseAddress[];
};

export type TUnifiedContactsSearchResponseSearchResult = {
  source?: string;
  subSource?: string;
  id?: string;
  displayName?: string;
  jobTitle?: string;
  department?: string;
  companyName?: string;
  mailAddresses?: string[];
  imAddresses?: string[];
  phoneNumbers?: TUnifiedContactsSearchResponsePhoneNumbers;
  addresses?: TUnifiedContactsSearchResponseAddresses;
  isFavorite?: boolean;
};

export type TUnifiedContactsSearchCache = {
  searchResults?: TUnifiedContactsSearchResponseSearchResult[];
  searchQuery?: string;
  searchTimestamp: number;
  sortSetting?: SortSetting;
};

export type TUnifiedContactsSearchResponse = {
  searchResult: TUnifiedContactsSearchResponseSearchResult[];
};

export type TUnifiedContactsPresenceResponse = {
  contactId: string;
  availability: string;
  activity: string;
};

export type TUnifiedContactsUserSettings = {
  selectedTileSize?: TILE_FORMAT;
  additionalPermissionsGrantSettingTimestamp?: number;
};

export type TUnifiedContactsImageResponse = {
  contactId: string;
  imageType: string;
  imageData: string;
};

export type TGeneralCheckConnection = {
  isAdminGranted: boolean;
  grantedPermissions: string[];
  notGrantedPermissions: string[];
  clientId: string;
};

export type TGeneralGetVersion = {
  version: string;
  edition: EUnifiedContactsEdition;
};

export type TGeneralGetAdminAppRegInfo = {
  clientId?: string;
  tenantId?: string;
};

export type TAdminGetDependenciesStatusDependency = {
  displayName: string;
  status: EDependencyStatus;
  statusDescription?: string;
};

export type TAdminGetDependenciesStatus = {
  dependencies: TAdminGetDependenciesStatusDependency[];
};

export type TAdminControllerGetManifestSettings = {
  displayName: string;
  shortDescription: string;
  longDescription: string;
  clientId: string;
  apiDomain: string;
};
export type TAdminControllerSetManifestSettingsPayload = {
  displayName: string;
  shortDescription: string;
  longDescription: string;
  apiDomain: string;
};

export type TAdminControllerGetVersionUpdateInfo = {
  updateAvailable?: boolean;
  updateInProgress?: boolean;
  restartRequired?: boolean;
  selectedChannel?: string;
  updateVersion?: string;
  currentVersion?: string;
  avaliableChannels?: string[];
  appServiceAzureUrl?: string;
};
export type TAdminControllerSetVersionUpdateSettings = {
  selectedReleaseChannel: string;
};
export type TAdminControllerGetIsUpdateInProgressResponse = {
  isUpdateInProgress: boolean;
  restartRequired: boolean;
};

export type TAdminControllerGetManifestInfo = {
  teamsManifestExists: boolean;
  teamsManifestUpdatePossible: boolean;
  teamsManifestVersion?: string;
  apiVersion?: string;
};

export type TAdminControllerGetAllSettingValuesOfCategoryValues = {
  settingId: string;
  value?: string;
};

export type TAdminControllerGetAllSettingValuesOfCategoryResponse = {
  categoryId: string;
  settings: TAdminControllerGetAllSettingValuesOfCategoryValues[];
};

export type TAdminPagePopoverSettings = {
  canClose: boolean;
  header: ReactNode;
  body: ReactNode;
  footer: ReactNode;
};
export type TAdminControllerGetSettingValueResponse = {
  value?: string;
};

export type TCancelablePromise<T> = {
  cancel: () => void;
  promise: Promise<T>;
};

export type TCancelablePromiseError = {
  isCanceled: boolean;
  error: any; // eslint-disable-line @typescript-eslint/no-explicit-any
};

export type Loadable<T> = {
  loading: boolean;
  data?: T;
};

export interface HashTable<T> {
  [key: string]: T;
}

export interface IAlertHandler {
  pushAlert(
    message: string,
    alertType: ALERT_TYPE,
    actions?: Slot<"div">,
    autoDisposeInMilliseconds?: number,
    disposable?: boolean
  ): string;
  updateAlert(
    id: string,
    message: string,
    alertType: ALERT_TYPE,
    actions?: any, // eslint-disable-line @typescript-eslint/no-explicit-any
    autoDisposeInMilliseconds?: number | undefined
  ): void;
  removeAlert(id: string): void;
  setTileActionAlert(alertText: string): void;
}

export type TUnfiedSearchState = {
  allSearchResults?: TUnifiedContactsSearchResponseSearchResult[];
  loading: boolean;
  sortSetting: SortSetting;
};

export type TSearchSourceLoading = {
  isUCSearchLoading: boolean;
  isAadLoading: boolean;
  isUserContactsLoading: boolean;
  isOrgContactsLoading: boolean;
  isSharePointLoading: boolean;
  isDatabaseLoading: boolean;
};

export type AlertMetaInfo = {
  id: string;
  message: string;
  alertType: ALERT_TYPE;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  actions?: any; // TODO will be replaced by new component https://github.com/microsoft/fluentui/issues/27949
  disposable: boolean;
};

export type TokenInfo = {
  oid: string;
  preferred_username: string;
};

export type SortSetting = {
  sortType: SORT_TYPE;
  sortCategory?: SORT_CATEGORY;
};

export type TGetBackendConfigResponse = {
  isDatabaseConfigured?: boolean;
};

export type TAdminSettingsValue = {
  value?: string;
  categoryId?: string;
  settingId?: string;
};

export type TAdminControllerSetAllSettingValuesOfCategorySetting = {
  value?: string;
  settingId?: string;
};

export type TAdminPageMetric = {
  $metricType: METRIC_TYPE;
  value: number;
  limit: number;
  displayName: string;
};

export type TAdminPageMetrics = {
  metrics: TAdminPageMetric[];
};  


export type TEntraIdFilter = {
  id?: string;
  filterAttribute: string;
  condition: string;
  filterValue: string;
  isValid?: boolean;
  validationMessage?: string;
  validationRunning?: boolean;
};

export type TEntraIdFilterState = {
  entraIdFilters: TEntraIdFilter[];
};
export type TFilterValidationResponse = { 
  statusCode: number;
  message: string;
};