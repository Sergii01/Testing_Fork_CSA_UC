import { AdminPageAuthentication } from "./pages/AdminPage/AdminPageAuthentication";
import { TeamsPage } from "./pages/TeamsPage/TeamsPage";
import { TeamsPageAlertServiceContextProvider } from "./providers/TeamsPageAlertServiceContextProvider";
import { PAGE_TYPE } from "./types/Enums";

const AppRoutes = [
  {
    index: true,
    element: <AdminPageAuthentication />,
  },
  {
    path: "/v1.3.0/favorites",
    element: <TeamsPageAlertServiceContextProvider><TeamsPage pageType={PAGE_TYPE.FAVORITES}/></TeamsPageAlertServiceContextProvider>,
  },
  {
    path: "/v1.3.0/search",
    element: <TeamsPageAlertServiceContextProvider><TeamsPage pageType={PAGE_TYPE.SEARCH}/></TeamsPageAlertServiceContextProvider>,
  },
  {
    path: "/contactSearchTab",
    element: <TeamsPageAlertServiceContextProvider><TeamsPage pageType={PAGE_TYPE.SEARCH}/></TeamsPageAlertServiceContextProvider>,
  },
];

export default AppRoutes;
