import { app } from "@microsoft/teams-js";
import { useCallback, useEffect, useRef, useState } from "react";
import localizedStrings from "../../loacalization/localization";
import {
  SORT_CATEGORY,
  SORT_TYPE,
  SOURCE,
} from "../../types/Enums";
import {
  getFavoritesControllerFavoritesOfTenant,
  postContactsControllerGetBatchContactImages,
  postContactsControllerGetBatchPresence,
} from "../../services/ApiService";
import {
  TGetBackendConfigResponse,
  TokenInfo,
  TUnfiedSearchState,
  TUnifiedContactsImageResponse,
  TUnifiedContactsPresenceResponse,
  TUnifiedContactsSearchResponseSearchResult,
} from "../../types/Types";
import { SearchResult } from "../../components/SearchResult";
import headerSmallTilted from "../../assets/images/headerSmallTilted.png";
import headerSmall from "../../assets/images/headerSmall.png";
import "../../assets/main.scss";
import { compareSearchResults } from "../../services/SortService";

export type FavoritesPageProps = {
  tenantId: string;
  backendConfig?: TGetBackendConfigResponse;
  currentUser: TokenInfo;
  updateSearchResultFavoriteState: (
    searchResultToUpdate: TUnifiedContactsSearchResponseSearchResult,
    newIsFavorite: boolean
  ) => Promise<void>;
};
export const FavoritesPage = (props: FavoritesPageProps) => {
  const CONTACT_PHOTO_REQUEST_BATCH_SIZE = 20;
  const PRESENCE_UPDATE_POLLING_INTERVAL_IN_MILLISECONDS = 10000;
  const [favorites, setFavorites] = useState<TUnfiedSearchState>({
    loading: true,
    allSearchResults: [],
    sortSetting: { sortType: SORT_TYPE.UNSORTED, sortCategory: undefined },
  });

  const [allPhotosResults, setAllPhotosResults] = useState<
    TUnifiedContactsImageResponse[]
  >([]);
  const [allPresenceResults, setAllPresenceResults] = useState<
    TUnifiedContactsPresenceResponse[]
  >([]);
  const pollingIntervalId = useRef<NodeJS.Timeout>();

  const updatePresence = useCallback(() => {
    const aadContactIds: string[] = [];
    allPresenceResults.forEach((presence) => {
      aadContactIds.push(presence.contactId);
    });
    const userPresence = postContactsControllerGetBatchPresence(aadContactIds);
    userPresence.then((value) => {
      setAllPresenceResults(value);
    });
  }, [allPresenceResults]);

  const getFavorites = useCallback((tenantId: string) => {
    try {
      if (tenantId) {
        const favResult = getFavoritesControllerFavoritesOfTenant(tenantId);
        favResult
          .then((value) => {
            setFavorites({
              loading: false,
              allSearchResults: value.searchResult.sort(compareSearchResults),
              sortSetting: {
                sortType: SORT_TYPE.ASCENDING,
                sortCategory: SORT_CATEGORY.DISPLAYNAME,
              },
            });
            const aadContactIds: string[] = [];
            value.searchResult.forEach((aadContact) => {
              if (aadContact.id && aadContact.source === SOURCE.AZURE_AD) {
                aadContactIds.push(aadContact.id);
              }
            });
            const userAadPhotoPromises = [];
            let imageResults: TUnifiedContactsImageResponse[] = [];
            for (
              let i = 0;
              i < aadContactIds.length;
              i += CONTACT_PHOTO_REQUEST_BATCH_SIZE
            ) {
              userAadPhotoPromises.push(
                postContactsControllerGetBatchContactImages(
                  aadContactIds.slice(i, i + CONTACT_PHOTO_REQUEST_BATCH_SIZE)
                )
              );
            }
            const userPresence =
              postContactsControllerGetBatchPresence(aadContactIds);
            userPresence.then((value) => {
              setAllPresenceResults(value);
            });

            Promise.allSettled(userAadPhotoPromises).then((results) => {
              results.forEach((result) => {
                if (result.status === "fulfilled") {
                  imageResults = imageResults.concat(result.value);
                } else {
                  console.warn(
                    "API-Call for getting photo failed",
                    result.reason
                  );
                }
              });
              setAllPhotosResults(imageResults);
            });
            const userContactPhotoPromises = [];
            const userContactIds: string[] = [];
            value.searchResult.forEach((userContact) => {
              if (
                userContact.id &&
                userContact.source === SOURCE.USER_CONTACT
              ) {
                userContactIds.push(userContact.id);
              }
            });
            for (
              let i = 0;
              i < userContactIds.length;
              i += CONTACT_PHOTO_REQUEST_BATCH_SIZE
            ) {
              userContactPhotoPromises.push(
                postContactsControllerGetBatchContactImages(
                  userContactIds.slice(i, i + CONTACT_PHOTO_REQUEST_BATCH_SIZE)
                )
              );
            }
            Promise.allSettled(userContactPhotoPromises).then((results) => {
              results.forEach((result) => {
                if (result.status === "fulfilled") {
                  imageResults = imageResults.concat(result.value);
                } else {
                  console.warn(
                    "API-Call for getting photo failed",
                    result.reason
                  );
                }
              });
              setAllPhotosResults(imageResults);
            });
          })
          .catch();
      }
    } catch (error) {
      console.error("Error while getting favorites", error);
    }
  }, []);
  const updateFavoriteFlagCallback = (
    contactId: string,
    newIsFavorite: boolean
  ): Promise<void> => {
    return new Promise<void>((resolve, reject) => {
      if (favorites.allSearchResults) {
        const searchResultToUpdate = favorites.allSearchResults.find(
          (e) => e.id === contactId
        );
        if (searchResultToUpdate) {
          props
            .updateSearchResultFavoriteState(
              searchResultToUpdate,
              newIsFavorite
            )
            .then(() => resolve())
            .catch(() => reject());
        }
      }
    });
  };

  const onFavoriteResultChange = (searchState: TUnfiedSearchState) => {
    setFavorites({
      loading: searchState.loading,
      allSearchResults: searchState.allSearchResults,
      sortSetting: searchState.sortSetting,
    });
  };

  useEffect(() => {
    // Update the document title using the browser API
    const isInitialized = app.isInitialized();
    if (!isInitialized) {
      app.initialize();
    }
    app
      .getContext()
      .then((context: app.Context) => {
        if (context.user) {
          if (context.user.tenant) {
            getFavorites(context.user?.tenant?.id);
          }
        }
        if (!isInitialized) {
          app.notifySuccess();
        }
      })
      .catch((error) => {
        app.notifyFailure(error);
      });
  }, [getFavorites]);

  useEffect(() => {
    pollingIntervalId.current = setInterval(
      updatePresence,
      PRESENCE_UPDATE_POLLING_INTERVAL_IN_MILLISECONDS
    );
    return () => {
      if (pollingIntervalId.current) {
        clearInterval(pollingIntervalId.current);
      }
    };
  }, [updatePresence]);
  return (
    <>
      <div className="header no-select">
        <img src={headerSmallTilted} className="header__image" alt="header" />
        <img src={headerSmall} className="header__image--mobile" alt="header" />
      </div>
      <div className="header__search-box">
        <p className="header__search-box__headline">
          {localizedStrings.searchPageHeadline}
        </p>
      </div>
      <div className="search-page__search-results__wrapper">
        <div className="farvorite-page__result-container">
          <SearchResult
            searchState={favorites}
            searchQuery={""}
            presence={allPresenceResults}
            photos={allPhotosResults}
            tenantId={props.tenantId}
            backendConfig={props.backendConfig}
            currentUser={props.currentUser}
            updateFavoriteFlagCallback={updateFavoriteFlagCallback}
            onSearchResultChange={onFavoriteResultChange}
          />
        </div>
      </div>
    </>
  );
};
