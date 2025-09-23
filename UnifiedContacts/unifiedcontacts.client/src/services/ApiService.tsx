import { HostClientType, authentication } from "@microsoft/teams-js";
import axios from "axios";
import { getMsalAdminPageInstance } from "../MsalSettings";
import {
  TAdminControllerGetIsUpdateInProgressResponse,
  TAdminControllerGetManifestSettings,
  TAdminControllerGetVersionUpdateInfo,
  TAdminControllerSetVersionUpdateSettings,
  TAdminControllerSetManifestSettingsPayload,
  TAdminGetDependenciesStatus,
  TGeneralCheckConnection,
  TGeneralGetVersion,
  TUnifiedContactsImageResponse,
  TUnifiedContactsPresenceResponse,
  TUnifiedContactsSearchResponse,
  TAdminControllerGetManifestInfo,
  TAdminControllerGetSettingValueResponse,
  TokenInfo,
  TGetBackendConfigResponse,
  TAdminSettingsValue,
  TAdminControllerGetAllSettingValuesOfCategoryResponse,
  TAdminControllerSetAllSettingValuesOfCategorySetting,
  TAdminPageMetrics,
  TEntraIdFilter,
  TFilterValidationResponse,
  TGeneralGetAdminAppRegInfo,
} from "../types/Types";

const ucApiUrl: string =
  window.location.protocol + "//" + window.location.host + "/v1.3.0/api";

export async function getGeneralControllerCheckConnection(): Promise<TGeneralCheckConnection> {
  const token = await authentication.getAuthToken();
  const response = await axios.get<TGeneralCheckConnection>(
    `${ucApiUrl}/general/connection`,
    { headers: { Authorization: `Bearer ${token}` } }
  );
  return response.data;
}

export async function getGeneralControllerGetVersion(): Promise<TGeneralGetVersion> {
  const response = await axios.get<TGeneralGetVersion>(
    `${ucApiUrl}/general/version`
  );
  return response.data;
}

export async function getBackendConfig(): Promise<TGetBackendConfigResponse> {
  const response = await axios.get<TGetBackendConfigResponse>(
    `${ucApiUrl}/general/backendconfig`
  );
  return response.data;
}

const convertClientType = (clientType: HostClientType): string => {
  switch (clientType) {
    case HostClientType.desktop:
    case HostClientType.macos:
      return "DESKTOP";
    case HostClientType.web:
      return "WEB";
    case HostClientType.android:
      return "ANDROID";
    case HostClientType.ios:
      return "IOS";
    default:
      return "UNKNOWN";
  }
};

export async function getSearchControllerSearchAzureAD(
  searchQuery: string,
  tenantId?: string,
  clientType?: HostClientType
): Promise<TUnifiedContactsSearchResponse | undefined> {
  const token = await authentication.getAuthToken();

  let url = `${ucApiUrl}/search/azuread?search=${searchQuery}`;
  if (tenantId) {
    url += `&tenantId=${tenantId}`;
  }
  if (clientType) {
    url += `&clientType=${convertClientType(clientType)}`;
  }
  const response = await axios.get<TUnifiedContactsSearchResponse>(url, {
    headers: { Authorization: `Bearer ${token}` },
  });

  // StatusCode 240 is the UnifiedContacts Status Code for signalizing that the source is disabled - In that case dismiss the result
  if (response.status === 240) {
    return undefined;
  }

  return response.data;
}

export async function getSearchControllerSearchUserContacts(
  searchQuery: string,
  tenantId?: string,
  clientType?: HostClientType
): Promise<TUnifiedContactsSearchResponse | undefined> {
  const token = await authentication.getAuthToken();

  let url = `${ucApiUrl}/search/usercontacts?search=${searchQuery}`;
  if (tenantId) {
    url += `&tenantId=${tenantId}`;
  }
  if (clientType) {
    url += `&clientType=${convertClientType(clientType)}`;
  }

  const response = await axios.get<TUnifiedContactsSearchResponse>(url, {
    headers: { Authorization: `Bearer ${token}` },
  });

  // StatusCode 240 is the UnifiedContacts Status Code for signalizing that the source is disabled - In that case dismiss the result
  if (response.status === 240) {
    return undefined;
  }

  return response.data;
}

export async function getSearchControllerSearchOrgContacts(
  searchQuery: string,
  tenantId?: string,
  clientType?: HostClientType
): Promise<TUnifiedContactsSearchResponse | undefined> {
  const token = await authentication.getAuthToken();

  let url = `${ucApiUrl}/search/orgcontacts?search=${searchQuery}`;
  if (tenantId) {
    url += `&tenantId=${tenantId}`;
  }
  if (clientType) {
    url += `&clientType=${convertClientType(clientType)}`;
  }

  const response = await axios.get<TUnifiedContactsSearchResponse>(url, {
    headers: { Authorization: `Bearer ${token}` },
  });

  // StatusCode 240 is the UnifiedContacts Status Code for signalizing that the source is disabled - In that case dismiss the result
  if (response.status === 240) {
    return undefined;
  }

  return response.data;
}

export async function getSearchControllerSearchSharePoint(
  searchQuery: string,
  tenantId?: string,
  clientType?: HostClientType
): Promise<TUnifiedContactsSearchResponse | undefined> {
  const token = await authentication.getAuthToken();

  let url = `${ucApiUrl}/search/sharepoint?search=${searchQuery}`;
  if (tenantId) {
    url += `&tenantId=${tenantId}`;
  }
  if (clientType) {
    url += `&clientType=${convertClientType(clientType)}`;
  }

  const response = await axios.get<TUnifiedContactsSearchResponse>(url, {
    headers: { Authorization: `Bearer ${token}` },
  });

  // StatusCode 240 is the UnifiedContacts Status Code for signalizing that the source is disabled - In that case dismiss the result
  if (response.status === 240) {
    return undefined;
  }

  return response.data;
}

export async function getSearchControllerSearchDatabase(
  searchQuery: string,
  tenantId?: string,
  clientType?: HostClientType
): Promise<TUnifiedContactsSearchResponse | undefined> {
  const token = await authentication.getAuthToken();

  let url = `${ucApiUrl}/search/database?search=${searchQuery}`;
  if (tenantId) {
    url += `&tenantId=${tenantId}`;
  }
  if (clientType) {
    url += `&clientType=${convertClientType(clientType)}`;
  }

  const response = await axios.get<TUnifiedContactsSearchResponse>(url, {
    headers: { Authorization: `Bearer ${token}` },
  });

  // StatusCode 240 is the UnifiedContacts Status Code for signalizing that the source is disabled - In that case dismiss the result
  if (response.status === 240) {
    return undefined;
  }

  return response.data;
}

export async function postContactsControllerGetBatchPresence(
  contactIds: string[]
): Promise<TUnifiedContactsPresenceResponse[]> {
  const token = await authentication.getAuthToken();
  const response = await axios.post<TUnifiedContactsPresenceResponse[]>(
    `${ucApiUrl}/contacts/presence`,
    { contactIds: contactIds },
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
  return response.data;
}

export async function postContactsControllerGetBatchContactImages(
  contactIds: string[]
): Promise<TUnifiedContactsImageResponse[]> {
  const token = await authentication.getAuthToken();
  const response = await axios.post<TUnifiedContactsImageResponse[]>(
    `${ucApiUrl}/contacts/photos`,
    { contactIds: contactIds },
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
  return response.data;
}

//Admin page calls

const getAdminAccessToken = async (): Promise<string> => {
  const msalAdminPageInstance = await getMsalAdminPageInstance();

  if (!msalAdminPageInstance) {
    throw new Error("Msal instance not initialized");
  }

  const accounts = msalAdminPageInstance.getAllAccounts();

  if (accounts.length === 0) {
    throw new Error("User not signed in");
  }
  const request = {
    scopes: [
      `api://${
        msalAdminPageInstance.getConfiguration().auth.clientId
      }/AdminPage.ReadWrite.All`,
    ],
    account: accounts[0],
  };

  const authResult = await msalAdminPageInstance
    .acquireTokenSilent(request)
    .catch((error: Error) => {
      console.warn("acquire token silently failed", error);
      msalAdminPageInstance.acquireTokenRedirect({
        scopes: [
          `api://${
            msalAdminPageInstance.getConfiguration().auth.clientId
          }/AdminPage.ReadWrite.All`,
        ],
      });
    });

  if (authResult) {
    return authResult.accessToken;
  } else {
    throw new Error("Auth not successfull");
  }
};

// AdminPage calls

export async function getGeneralAdminPageInfo(): Promise<TGeneralGetAdminAppRegInfo> {
  const response = await axios.get<TGeneralGetAdminAppRegInfo>(
    `${ucApiUrl}/general/adminpage/appreginfo`
  );
  return response.data;
}

export async function getAdminControllerGetDependenciesStatus(): Promise<TAdminGetDependenciesStatus> {
  const token = await getAdminAccessToken();
  const response = await axios.get<TAdminGetDependenciesStatus>(
    `${ucApiUrl}/admin/dependencies/status`,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );

  return response.data;
}

export async function getAdminControllerGetManifestSettings(): Promise<TAdminControllerGetManifestSettings> {
  const token = await getAdminAccessToken();
  const response = await axios.get<TAdminControllerGetManifestSettings>(
    `${ucApiUrl}/admin/manifest/settings`,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );

  return response.data;
}

export async function postAdminControllerSetManifestSettings(
  payload: TAdminControllerSetManifestSettingsPayload
): Promise<void> {
  const token = await getAdminAccessToken();
  await axios.post<void>(`${ucApiUrl}/admin/manifest/settings`, payload, {
    headers: { Authorization: `Bearer ${token}` },
  });
}

export async function getAdminControllerGetVersionUpdateInfo(): Promise<TAdminControllerGetVersionUpdateInfo> {
  const token = await getAdminAccessToken();
  const response = await axios.get<TAdminControllerGetVersionUpdateInfo>(
    `${ucApiUrl}/admin/version/update`,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );

  return response.data;
}

export async function putAdminControllerSetVersionUpdateSettingsResponse(
  payload: TAdminControllerSetVersionUpdateSettings
): Promise<void> {
  const token = await getAdminAccessToken();
  await axios.put(`${ucApiUrl}/admin/version/update/settings`, payload, {
    headers: { Authorization: `Bearer ${token}` },
  });
}

export async function postAdminControllerUpdateUnifiedContacts(): Promise<void> {
  const token = await getAdminAccessToken();
  await axios.post<TUnifiedContactsImageResponse[]>(
    `${ucApiUrl}/admin/version/update`,
    undefined,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
}

export async function getAdminControllerGetIsUpdateInProgress(): Promise<TAdminControllerGetIsUpdateInProgressResponse> {
  const token = await getAdminAccessToken();
  const response =
    await axios.get<TAdminControllerGetIsUpdateInProgressResponse>(
      `${ucApiUrl}/admin/version/update/progress`,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );

  return response.data;
}

export async function getAdminControllerGetManifestInfo(): Promise<TAdminControllerGetManifestInfo> {
  const token = await getAdminAccessToken();
  const response = await axios.get<TAdminControllerGetManifestInfo>(
    `${ucApiUrl}/admin/manifest`,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );

  return response.data;
}

export async function postAdminControllerUploadManifestInfo(): Promise<void> {
  const token = await getAdminAccessToken();
  const response = await axios.post<void>(
    `${ucApiUrl}/admin/manifest`,
    undefined,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );

  return response.data;
}

export async function getAdminControllerGetSettingsValue(
  categoryId: string,
  settingId: string
): Promise<TAdminSettingsValue> {
  const token = await getAdminAccessToken();
  const response = await axios.get<TAdminControllerGetSettingValueResponse>(
    `${ucApiUrl}/admin/settings/${categoryId}/${settingId}`,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );

  return {
    categoryId: categoryId,
    settingId: settingId,
    value: response.data.value,
  };
}

export async function getAdminControllerGetAllSettingValuesOfCategory(
  categoryId: string
): Promise<TAdminControllerGetAllSettingValuesOfCategoryResponse> {
  const token = await getAdminAccessToken();
  const response =
    await axios.get<TAdminControllerGetAllSettingValuesOfCategoryResponse>(
      `${ucApiUrl}/admin/settings/${categoryId}`,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );

  return response.data;
}

export async function postAdminControllerSetAllSettingValuesOfCategory(
  categoryId: string,
  settings: TAdminControllerSetAllSettingValuesOfCategorySetting[],
  updateRuntimeInfo = false
): Promise<void> {
  let uri = `${ucApiUrl}/admin/settings/${categoryId}`;
  if (updateRuntimeInfo) {
    uri += "?updateRuntimeInfo=true";
  }

  const token = await getAdminAccessToken();
  await axios.post<TAdminControllerGetSettingValueResponse>(
    uri,
    {
      values: settings,
    },
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
}

export async function putAdminControllerSetEntraIdFilterSettings(
  filterId: string,
  filter: TEntraIdFilter
): Promise<void> {
  const token = await getAdminAccessToken();
  await axios.put<TAdminControllerGetSettingValueResponse>(
    `${ucApiUrl}/admin/settings/filterattributes/entraIdfilters/${filterId}`,
    filter,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
}

export async function postAdminControllerCreateEntraIdFilter(
  filter: TEntraIdFilter
) {
  const token = await getAdminAccessToken();
  await axios.post<TFilterValidationResponse>(
    `${ucApiUrl}/admin/settings/filterattributes/entraIdfilters`,
    filter,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
}

export async function deleteAdminControllerDeleteEntraIdFilter(
  filterId: string
) {
  const token = await getAdminAccessToken();
  await axios.delete(
    `${ucApiUrl}/admin/settings/filterattributes/entraIdfilters/${filterId}`,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
}

export async function postAdminControllerSetSettingsValue(
  categoryId: string,
  settingId: string,
  value?: string,
  updateRuntimeInfo = false
): Promise<void> {
  let uri = `${ucApiUrl}/admin/settings/${categoryId}/${settingId}`;
  if (updateRuntimeInfo) {
    uri += "?updateRuntimeInfo=true";
  }

  const token = await getAdminAccessToken();
  await axios.post<TAdminControllerGetSettingValueResponse>(
    uri,
    {
      value: value,
    },
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
}

//Favorites calls

export async function getFavoritesControllerFavoritesOfTenant(
  tenantId: string
): Promise<TUnifiedContactsSearchResponse> {
  const token = await authentication.getAuthToken();
  const response = await axios.get<TUnifiedContactsSearchResponse>(
    `${ucApiUrl}/favorites/tenants/${tenantId}/contacts`,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
  return response.data;
}

export async function putFavoritesControllerAddFavorite(
  tenantId: string,
  contactId: string
): Promise<void> {
  const token = await authentication.getAuthToken();
  await axios.put(
    `${ucApiUrl}/favorites/tenants/${tenantId}/contacts/${contactId}`,
    {
      tenantId: tenantId,
      contactId: contactId,
    },
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
}

export async function deleteFavoritesControllerDeleteFavorite(
  tenantId: string,
  contactId: string
): Promise<void> {
  const token = await authentication.getAuthToken();
  await axios.delete(
    `${ucApiUrl}/favorites/tenants/${tenantId}/contacts/${contactId}`,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
}

export async function getUserToken(): Promise<TokenInfo> {
  const token = await authentication.getAuthToken();
  //parse JWT token into JSON
  const jsonPayload = decodeURIComponent(
    window
      .atob(token.split(".")[1].replace(/-/g, "+").replace(/_/g, "/"))
      .split("")
      .map(function (c) {
        return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
      })
      .join("")
  );
  return JSON.parse(jsonPayload);
}

export const getMetrics = async (): Promise<TAdminPageMetrics> => {
  const token = await getAdminAccessToken();
  const response = await axios.get<TAdminPageMetrics>(
    `${ucApiUrl}/admin/metrics`,
    {
      headers: { Authorization: `Bearer ${token}` },
    }
  );
  return response.data;
};
