export enum TILE_FORMAT {
  LARGE = "LARGE",
  SMALL = "SMALL",
}

export enum SOURCE {
  AZURE_AD = "AZURE_AD",
  SHAREPOINT = "SHAREPOINT",
  USER_CONTACT = "USER_CONTACT",
  ORG_CONTACT = "ORG_CONTACT",
  DATABASE = "DATABASE",
}

export enum EDependencyStatus {
  HEALTHY = "HEALTHY",
  WARNING = "WARNING",
  ERROR = "ERROR",
  UNKNOWN = "UNKNOWN",
}


export enum EAdminGrantCheckStatus {
  GRANTED,
  NOT_ADMIN_GRANTEND,
  UNAUTHORIZED,
  UNKNOWN,
  NOT_ALL_PERMISSIONS_GRANTED,
}

export enum EUnifiedContactsEdition {
  FREE = "Free",
  PRO = "Pro",
  COMMUNITY_EDITION = "Community Edition",
}

export enum ALERT_TYPE {
  INFO = "info",
  SUCCESS = "success",
  WARNING = "warning",
  ERROR = "error",
}

export enum PAGE_TYPE {
  FAVORITES = "favorites",
  SEARCH = "search",
}

export enum SORT_TYPE {
  UNSORTED = "UNSORTED",
  ASCENDING = "ASCENDING",
  DESCENDING = "DESCENDING",
}

export enum SORT_CATEGORY {
  DISPLAYNAME = "DISPLAYNAME",
}

export enum TILE_ACTIONS {
  MESSAGE = "MESSAGE",
  VIDEOCALL = "VIDEOCALL",
  PHONECALL = "PHONECALL",
  MAILTO = "MAILTO",
}

export enum METRIC_TYPE {
  QUOTA = "quota",
  DISPLAY_NUMBER = "displayNumber",
}

export enum PROGRESS_BAR_VARIANT {
  INFO = "info",
  SUCCESS = "success",
  WARNING = "warning",
  DANGER = "danger",
}
