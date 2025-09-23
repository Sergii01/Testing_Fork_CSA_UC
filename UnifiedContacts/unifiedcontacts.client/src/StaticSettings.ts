// An abstract ts class is used instead of a json, as a json might be loaded in as an asset in run time. As we want to detect caching in the frontend we need to make sure that this file is treated as a regular project file.
export abstract class StaticSettings {
  static version = "/INTERNAL_BUILD/";
  static adminGrantUrl =
    "https://login.microsoftonline.com/common/adminconsent?client_id=[[clientId]]";
  static adminGrantUrlFree =
    "https://login.microsoftonline.com/common/adminconsent?client_id=[[clientId]]&redirect_uri=https://glueckkanja-gab.com/";
}
