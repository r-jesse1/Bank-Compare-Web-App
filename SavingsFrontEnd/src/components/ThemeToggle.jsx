import {
  useMantineColorScheme,
  ActionIcon,
  useComputedColorScheme,
} from "@mantine/core";

import { IconSun, IconMoon } from "@tabler/icons-react";

function ThemeToggle() {
  const { setColorScheme } = useMantineColorScheme();
  const computedColorScheme = useComputedColorScheme("light", {
    getInitialValueInEffect: true,
  });

  return (
    <ActionIcon
      className="toggle-theme-button"
      onClick={() =>
        setColorScheme(computedColorScheme === "light" ? "dark" : "light")
      }
      variant="default"
      size="lg"
      aria-label="Toggle color scheme"
    >
      {computedColorScheme === "light" ? (
        <IconMoon stroke={1.5} />
      ) : (
        <IconSun stroke={1.5} />
      )}
    </ActionIcon>
  );
}

export default ThemeToggle;
