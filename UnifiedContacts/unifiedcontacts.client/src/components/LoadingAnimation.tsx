import { useLottie } from "lottie-react";
import LoadingAnimationJson from "../assets/animations/UC-loader.json";

export function LoadingAnimation() {
  const OPTIONS = {
    animationData: LoadingAnimationJson,
    loop: true,
  };

  const { View } = useLottie(OPTIONS, { height: "inherit" });

  return <>{View}</>;
}
