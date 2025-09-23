import { SORT_TYPE, TILE_FORMAT } from "../types/Enums";
import {
  SortSetting,
  TUnifiedContactsImageResponse,
  TUnifiedContactsSearchCache,
  TUnifiedContactsSearchResponseSearchResult,
  TUnifiedContactsUserSettings,
} from "../types/Types";

const SEARCH_CACHE_IDENTIFIER = `ucSearchCache_v3`;
const CONTACT_IMAGE_CACHE_IDENTIFIER = `ucContactImageCache_v3`;
const USER_SETTINGS_IDENTIFIER = "ucUserSettings_v3";
const SEARCH_CACHE_TIMEOUT_HOURS = 2;
const USER_SETTINGS_TIMEOUT_DAYS = 7;

class CachingService {
  private static _instance: CachingService;
  private searchCacheCache: { [id: string]: TUnifiedContactsSearchCache };
  private imageCacheCache: { [id: string]: TUnifiedContactsImageResponse[] };
  private userSettingsCache?: TUnifiedContactsUserSettings;
  private constructor() {
    this.searchCacheCache = {};
    this.imageCacheCache = {};
    this.userSettingsCache = undefined;
  }

  public static get Instance() {
    if (!this._instance) {
      this._instance = new this();
    }
    return this._instance;
  }

  public getUserSettings = (): TUnifiedContactsUserSettings => {
    if (!this.userSettingsCache) {
      const userSettingsString = localStorage.getItem(USER_SETTINGS_IDENTIFIER);
      if (userSettingsString) {
        this.userSettingsCache = JSON.parse(userSettingsString);
      }
    }
    if (this.userSettingsCache) {
      return this.userSettingsCache;
    } else {
      return { selectedTileSize: undefined };
    }
  };

  public setUserSettings = (
    newUserSettings: TUnifiedContactsUserSettings
  ): void => {
    this.userSettingsCache = newUserSettings;
    localStorage.setItem(
      USER_SETTINGS_IDENTIFIER,
      JSON.stringify(newUserSettings)
    );
  };

  public setTileSizeUserSetting = (newTileSize?: TILE_FORMAT) => {
    const userSettings = this.getUserSettings();
    userSettings.selectedTileSize = newTileSize;
    this.setUserSettings(userSettings);
  };

  /**
   * Gets the search cache.
   * @param tenantId tenantId of the user.
   * @returns The cache from the tenantId or undefined if it doesn't exist.
   */
  public getSearchCache = (
    tenantId: string
  ): TUnifiedContactsSearchCache | undefined => {
    const searchCacheLocalStorageKey = `${SEARCH_CACHE_IDENTIFIER}_${tenantId}`;
    let searchCache: TUnifiedContactsSearchCache | undefined = undefined;

    // This check is necessary as the linter and vs do think that this.searchCacheCache[searchCacheLocalStorageKey] can not be undefined - but it can be.
    if (this.searchCacheCache[searchCacheLocalStorageKey]) {
      searchCache = this.searchCacheCache[searchCacheLocalStorageKey];
    }

    if (!searchCache) {
      const searchCacheString = localStorage.getItem(searchCacheLocalStorageKey);
      if (searchCacheString) {
        const searchCache = JSON.parse(searchCacheString);
        if (searchCache) {
          this.searchCacheCache[searchCacheLocalStorageKey] = searchCache;
        }
      }
    }

    if (
      searchCache?.searchTimestamp &&
      Date.now() - searchCache.searchTimestamp >
        SEARCH_CACHE_TIMEOUT_HOURS * 60 * 60 * 1000
    ) {
      this.clearSearchCache(tenantId);
      return undefined;
    }

    return searchCache;
  };

  public setSearchCache = (
    tenantId: string,
    newSearchCache: TUnifiedContactsSearchCache
  ): void => {
    this.searchCacheCache[`${SEARCH_CACHE_IDENTIFIER}_${tenantId}`] =
      newSearchCache;
    localStorage.setItem(
      `${SEARCH_CACHE_IDENTIFIER}_${tenantId}`,
      JSON.stringify(newSearchCache)
    );
  };

  public clearSearchCache = (tenantId: string): void => {
    delete this.searchCacheCache[`${SEARCH_CACHE_IDENTIFIER}_${tenantId}`];
    localStorage.removeItem(`${SEARCH_CACHE_IDENTIFIER}_${tenantId}`);
  };

  public getContactImageCache = (
    tenantId: string
  ): TUnifiedContactsImageResponse[] | undefined => {
    const imageCacheLocalStorageKey = `${CONTACT_IMAGE_CACHE_IDENTIFIER}_${tenantId}`;
    if (this.imageCacheCache[imageCacheLocalStorageKey]) {
      return this.imageCacheCache[imageCacheLocalStorageKey];
    }
    const imageCacheString = localStorage.getItem(imageCacheLocalStorageKey);
    if (imageCacheString) {
      const imageCache: TUnifiedContactsImageResponse[] =
        JSON.parse(imageCacheString);
      this.imageCacheCache[imageCacheLocalStorageKey] = imageCache;
      return imageCache;
    }
    return undefined;
  };

  public setContactImageCache = (
    tenantId: string,
    newContactImageCache: TUnifiedContactsImageResponse[]
  ): void => {
    const imageCache = newContactImageCache;

    this.imageCacheCache[`${CONTACT_IMAGE_CACHE_IDENTIFIER}_${tenantId}`] =
      imageCache;
    localStorage.setItem(
      `${CONTACT_IMAGE_CACHE_IDENTIFIER}_${tenantId}`,
      JSON.stringify(imageCache)
    );
  };

  public getCachedSearchResults = (
    tenantId: string
  ): TUnifiedContactsSearchResponseSearchResult[] | undefined => {
    const searchCache = this.getSearchCache(tenantId);
    if (searchCache && searchCache.searchResults) {
      return searchCache.searchResults;
    }
    return undefined;
  };

  public getCachedSortSetting = (tenantId: string): SortSetting => {
    const searchCache = this.getSearchCache(tenantId);
    if (searchCache && searchCache.sortSetting) {
      return searchCache.sortSetting;
    }
    return {
      sortType: SORT_TYPE.UNSORTED,
      sortCategory: undefined,
    };
  };

  public setCachedSearchResults = (
    tenantId: string,
    newSearchResults: TUnifiedContactsSearchResponseSearchResult[] | undefined,
    sortSetting: SortSetting | undefined
  ): void => {
    if (newSearchResults === undefined) {
      this.clearSearchCache(tenantId);
      return;
    }

    let searchCache = this.getSearchCache(tenantId);
    if (searchCache) {
      searchCache.searchResults = newSearchResults;
      searchCache.sortSetting = sortSetting;
      searchCache.searchTimestamp = Date.now();
    } else {
      searchCache = {
        searchResults: newSearchResults,
        searchQuery: undefined,
        searchTimestamp: Date.now(),
        sortSetting: sortSetting,
      };
    }

    this.setSearchCache(tenantId, searchCache);
  };

  public getCachedSearchQuery = (tenantId: string): string | undefined => {
    const searchCache = this.getSearchCache(tenantId);
    if (searchCache && searchCache.searchQuery) {
      return searchCache.searchQuery;
    }
    return undefined;
  };

  public setCachedSearchQuery = (
    tenantId: string,
    newSearchQuery: string
  ): void => {
    let searchCache = this.getSearchCache(tenantId);
    if (searchCache) {
      searchCache.searchQuery = newSearchQuery;
      searchCache.searchTimestamp = Date.now();
    } else {
      searchCache = {
        searchResults: undefined,
        searchQuery: newSearchQuery,
        searchTimestamp: Date.now(),
      };
    }

    this.setSearchCache(tenantId, searchCache);
  };
  public setAdditionalPermissionsGrantSettingTimestamp = () => {
    const userSettings = this.getUserSettings();
    userSettings.additionalPermissionsGrantSettingTimestamp = Date.now();
    this.setUserSettings(userSettings);
  };
  public clearAdditionalPermissionsGrantSettingTimestamp = () => {
    const userSettings = this.getUserSettings();
    userSettings.additionalPermissionsGrantSettingTimestamp = undefined;
    this.setUserSettings(userSettings);
  };
  public getAdditionalPermissionsGrantSetting = (): boolean => {
    const userSettings = this.getUserSettings();
    //returns true if hideOrgContactsSettingTimestamp is set and is within the last USER_SETTINGS_TIMEOUT_DAYS (7 days)
    return (
      userSettings.additionalPermissionsGrantSettingTimestamp !== undefined &&
      Date.now() - userSettings.additionalPermissionsGrantSettingTimestamp <
      USER_SETTINGS_TIMEOUT_DAYS * 24 * 60 * 60 * 1000
    )
  };
}

const cachingService = CachingService.Instance;

export { cachingService };
