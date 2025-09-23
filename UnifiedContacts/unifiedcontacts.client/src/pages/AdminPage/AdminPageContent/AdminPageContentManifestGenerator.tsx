import { ChangeEvent, useEffect, useState } from "react";
import Spinner from "react-bootstrap/Spinner";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import {
  getAdminControllerGetManifestSettings,
  postAdminControllerSetManifestSettings,
} from "../../../services/ApiService";
import { TAdminControllerGetManifestInfo } from "../../../types/Types";
import Alert from "react-bootstrap/Alert";

export type AdminPageContentManifestGeneratorProps = {
  teamsManifestInfo?: TAdminControllerGetManifestInfo;
  uploadManifest: () => Promise<void>;
};

export function AdminPageContentManifestGenerator(
  props: AdminPageContentManifestGeneratorProps
) {
  const [loading, setLoading] = useState<boolean>(true);
  const [manifestSaveLoading, setManifestSaveLoading] =
    useState<boolean>(false);
  const [uploadManifestLoading, setUploadManifestLoading] =
    useState<boolean>(false);
  const [appDisplayName, setAppDisplayName] = useState<string>("");
  const [appShortDescription, setAppShortDescription] = useState<string>("");
  const [appLongDescription, setAppLongDescription] = useState<string>("");
  const [appClientId, setAppClientId] = useState<string>("");
  const [apiDomain, setApiDomain] = useState<string>("");

  function saveManifestSettings(): Promise<void> {
    if (
      isFormValid(
        appDisplayName,
        appShortDescription,
        appLongDescription,
        apiDomain
      )
    ) {
      setManifestSaveLoading(true);
      const promise = postAdminControllerSetManifestSettings({
        displayName: appDisplayName,
        shortDescription: appShortDescription,
        longDescription: appLongDescription,
        apiDomain: apiDomain,
      });
      promise
        .then(() => {
          setManifestSaveLoading(false);
        })
        .catch(() => {
          setManifestSaveLoading(false);
        });

      return promise;
    }
    return new Promise((_resolve, reject) => reject("Form not valid"));
  }

  function loadManifestInfo() {
    getAdminControllerGetManifestSettings().then((res) => {
      setAppDisplayName(res.displayName);
      setAppShortDescription(res.shortDescription);
      setAppLongDescription(res.longDescription);
      setAppClientId(res.clientId);
      setApiDomain(res.apiDomain || window.location.hostname);
      setLoading(false);
    });
  }

  useEffect(() => {
    loadManifestInfo();
  }, []);

  function isDisplayNameValid(displayName: string): boolean {
    return (
      displayName !== undefined &&
      displayName.length <= 30 &&
      displayName.length > 0
    );
  }

  function isShortDescriptionValid(shortDescription: string): boolean {
    return (
      shortDescription !== undefined &&
      shortDescription.length <= 80 &&
      shortDescription.length > 0
    );
  }

  function isLongDescriptionValid(longDescription: string): boolean {
    return (
      longDescription !== undefined &&
      longDescription.length <= 4000 &&
      longDescription.length > 0
    );
  }

  function isDomainValid(domain: string): boolean {
    return (
      domain !== undefined &&
      domain.length <= 1500 &&
      domain.length > 0 &&
      !domain.includes("/")
    );
  }

  function isFormValid(
    displayName: string,
    shortDescription: string,
    longDescription: string,
    domain: string
  ): boolean {
    return (
      isDisplayNameValid(displayName) &&
      isShortDescriptionValid(shortDescription) &&
      isLongDescriptionValid(longDescription) &&
      isDomainValid(domain)
    );
  }

  return (
    <>
      <h1>Teams Manifest Settings</h1>
      {loading ? (
        <Spinner />
      ) : (
        <>
          <Alert variant="primary">
            <b>NOTE:</b> These settings can only be applied (published) on a new
            version update of Unified Contacts. Be sure to configure your
            preferences <b>before</b> clicking the &quot;Publish&quot; button.
          </Alert>
          <Form
            onSubmit={(e) => {
              e.preventDefault();
            }}
          >
            <Form.Group className="mb-3" controlId="appDisplayName">
              <Form.Label>Display Name</Form.Label>
              <Form.Control
                type="text"
                placeholder="Display name"
                value={appDisplayName}
                onChange={(
                  val: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
                ) => setAppDisplayName(val.target.value || "")}
                isInvalid={!isDisplayNameValid(appDisplayName)}
              />
              <Form.Control.Feedback type="invalid">
                Please choose a display name. The maximum length is 30.
              </Form.Control.Feedback>
              <Form.Text className="text-muted">
                The app will appear in the Teams app under this name
              </Form.Text>
            </Form.Group>
            <Form.Group className="mb-3" controlId="appShortDescription">
              <Form.Label>Short Description</Form.Label>
              <Form.Control
                type="text"
                placeholder="Short description"
                disabled
                value={appShortDescription}
                onChange={(
                  val: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
                ) => setAppShortDescription(val.target.value || "")}
                isInvalid={!isShortDescriptionValid(appShortDescription)}
              />
              <Form.Control.Feedback type="invalid">
                Please choose a short description. The maximum length is 80.
              </Form.Control.Feedback>
              <Form.Text className="text-muted">
                This description is shown when hovered over the Teams app
              </Form.Text>
            </Form.Group>
            <Form.Group className="mb-3" controlId="appLongDescription">
              <Form.Label>Long Description</Form.Label>
              <Form.Control
                type="text"
                placeholder="Long description"
                as="textarea"
                disabled
                rows={8}
                value={appLongDescription}
                onChange={(
                  val: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
                ) => setAppLongDescription(val.target.value || "")}
                isInvalid={!isLongDescriptionValid(appLongDescription)}
              />
              <Form.Control.Feedback type="invalid">
                Please choose a long description. The maximum length is 4000.
              </Form.Control.Feedback>
              <Form.Text className="text-muted">
                This description is shown in the Teams app store
              </Form.Text>
            </Form.Group>
            <Form.Group className="mb-3" controlId="appClientId">
              <Form.Label>App Registration Client Id</Form.Label>
              <Form.Control
                type="text"
                placeholder="App Registration Client Id"
                disabled
                value={appClientId}
                onChange={(
                  val: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
                ) => setAppClientId(val.target.value || "")}
              />
              <Form.Text className="text-muted">
                ClientId of the Unified Contacts AppRegistration
              </Form.Text>
            </Form.Group>
            <Form.Group className="mb-3" controlId="apiDomain">
              <Form.Label>Api Domain</Form.Label>
              <Form.Control
                type="text"
                placeholder="Api Domain"
                value={apiDomain}
                onChange={(
                  val: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
                ) => setApiDomain(val.target.value || "")}
                isInvalid={!isDomainValid(apiDomain)}
              />
              <Form.Control.Feedback type="invalid">
                Please choose a domain.
              </Form.Control.Feedback>
              <Form.Text className="text-muted">
                Domain of the Unified Contacts backend
              </Form.Text>
            </Form.Group>
            <Button
              className="admin-page__manifest-generator"
              variant="primary"
              onClick={saveManifestSettings}
              disabled={
                !isFormValid(
                  appDisplayName,
                  appShortDescription,
                  appLongDescription,
                  apiDomain
                ) || manifestSaveLoading
              }
            >
              {manifestSaveLoading ? (
                <Spinner animation="border" size="sm" />
              ) : (
                "Save"
              )}
            </Button>
            <Button
              className="admin-page__manifest-generator"
              disabled={
                !isFormValid(
                  appDisplayName,
                  appShortDescription,
                  appLongDescription,
                  apiDomain
                ) ||
                uploadManifestLoading ||
                !props.teamsManifestInfo ||
                !props.teamsManifestInfo.teamsManifestUpdatePossible
              }
              onClick={() => {
                if (
                  isFormValid(
                    appDisplayName,
                    appShortDescription,
                    appLongDescription,
                    apiDomain
                  )
                ) {
                  setUploadManifestLoading(true);
                  saveManifestSettings().then(() =>
                    props.uploadManifest().then(() => {
                      setUploadManifestLoading(false);
                    })
                  );
                }
              }}
            >
              {uploadManifestLoading ? (
                <Spinner animation="border" size="sm" />
              ) : (
                "Publish"
              )}
            </Button>
          </Form>
        </>
      )}
    </>
  );
}
