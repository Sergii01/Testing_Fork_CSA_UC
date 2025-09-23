import { TUnifiedContactsSearchResponseSearchResult } from "../types/Types";

export const compareSearchResults = (
  a: TUnifiedContactsSearchResponseSearchResult,
  b: TUnifiedContactsSearchResponseSearchResult
) => {
  if(a.displayName === b.displayName) {	
    return 0;
  }
  if(!b.displayName) {
    return -1;
  }
  if(!a.displayName) {
    return 1;
  }
  return a.displayName.localeCompare(b.displayName);
};
