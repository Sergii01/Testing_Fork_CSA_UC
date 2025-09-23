import { TCancelablePromise, TCancelablePromiseError } from "../types/Types";

export function getCancelablePromise<T>(
  promise: Promise<T>
): TCancelablePromise<T> {
  let hasCanceled_ = false;

  const wrappedPromise = new Promise<T>((resolve, reject) => {
    promise.then(
      (val) => {
        if (hasCanceled_) {
          const canceledResponse: TCancelablePromiseError = {
            isCanceled: true,
            error: undefined,
          };
          reject(canceledResponse);
        } else {
          resolve(val);
        }
      },
      (error) => {
        const response: TCancelablePromiseError = {
          isCanceled: hasCanceled_,
          error: error,
        };
        reject(response);
      }
    );
  });

  return {
    promise: wrappedPromise,
    cancel() {
      hasCanceled_ = true;
    },
  };
}