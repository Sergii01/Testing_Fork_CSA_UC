declare global {
  interface String {
    IsNullOrEmpty(): boolean;
  }
  //necessary to monkey patch Array
  //eslint-disable-next-line  @typescript-eslint/no-unused-vars
  interface Array<T> {
    IsNullOrEmpty(): boolean;
  }
}
// String prototype is read only, properties should not be added
// but is needed to monkey patch
String.prototype.IsNullOrEmpty = function (): boolean {
  return !this || this.length === 0;
};
// Array prototype is read only, properties should not be added
// but is needed to monkey patch
Array.prototype.IsNullOrEmpty = function (): boolean {
  return !this || this.length === 0;
};
export {};
