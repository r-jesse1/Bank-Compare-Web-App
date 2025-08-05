import "./App.css";

import "@mantine/core/styles.css";
import "@mantine/charts/styles.css";
import { shadcnCssVariableResolver } from "./cssVariableResolver.ts";
import { shadcnTheme } from "./theme.ts";
import { MantineProvider } from "@mantine/core";

import ThemeToggle from "./components/ThemeToggle";
import { Savings } from "./Savings";
import { SavingsRateChartModal } from "./components/SavingsRateChartModal";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

function App() {
  return (
    <MantineProvider
      theme={shadcnTheme}
      cssVariablesResolver={shadcnCssVariableResolver}
    >
      <Router>
        <ThemeToggle />
        <Routes>
          <Route path="/" element={<Savings />} />
          <Route path="/test" element={<SavingsRateChartModal />} />
        </Routes>
      </Router>
    </MantineProvider>
  );
}

export default App;
