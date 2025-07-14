import {
  Card,
  Text,
  Badge,
  Stack,
  Container,
  Anchor,
  Title,
  Group,
  Grid,
  Button,
  Image,
  Checkbox,
  SegmentedControl,
  Pagination,
  Paper,
  TextInput,
  Select,
  Switch,
  ThemeIcon,
  MultiSelect,
} from "@mantine/core";

import {
  IconCurrencyDollar,
  IconRepeat,
  IconCalendar,
  IconPig,
  IconSchool,
  IconHospital,
} from "@tabler/icons-react";

import {
  MantineProvider,
  useMantineColorScheme,
  ActionIcon,
  useComputedColorScheme,
} from "@mantine/core";
import "./App.css";

import { IconSun, IconMoon } from "@tabler/icons-react";
import { SavingsCard } from "./SavingsCard";

import { useEffect, useState, useRef, useCallback } from "react";

export function Savings() {
  const [savings, setSavings] = useState([]);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [sortBy, setSortBy] = useState("TotalRate desc");
  const observer = useRef();

  const { setColorScheme } = useMantineColorScheme();
  const computedColorScheme = useComputedColorScheme("light", {
    getInitialValueInEffect: true,
  });

  const lastElementRef = useCallback(
    (node) => {
      if (observer.current) observer.current.disconnect();

      observer.current = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting && hasMore) {
          setPage((prev) => prev + 1);
        }
      });

      if (node) observer.current.observe(node);
    },
    [hasMore]
  );

  const fetchSavings = async (pageNumber) => {
    try {
      const bankList = ["ANZ", "NAB", "Westpac"];

      const params = new URLSearchParams({
        pageNumber: pageNumber.toString(),
        pageSize: "10",
        sortBy: sortBy,
      });
      bankList.forEach((bank) => params.append("banks", bank));

      const response = await fetch(
        `http://localhost:5134/savingsrate?${params.toString()}`
      );
      const data = await response.json();

      if (data.length === 0) {
        setHasMore(false);
      } else {
        setSavings((prev) => [...prev, ...data]);
      }
    } catch (e) {
      console.error(e);
    }
  };

  useEffect(() => {
    fetchSavings(page);
  }, [page, sortBy]);

  function changeSort(sort) {
    setSavings([]);
    setPage(1);
    setSortBy(sort);
  }

  return (
    <Container size="xl" mt="lg">
      <ActionIcon
        className="toggle-theme-button"
        onClick={() =>
          setColorScheme(computedColorScheme === "light" ? "dark" : "light")
        }
        variant="default"
        size="lg"
        aria-label="Toggle color scheme"
      >
        <IconSun stroke={1.5} />
        <IconMoon stroke={1.5} />
      </ActionIcon>

      <Title order={2} mb="md">
        Savings Accounts
      </Title>

      <Paper
        shadow="md"
        p="md"
        withBorder
        style={{ display: "flex", alignItems: "center", gap: "lg" }}
        mb={20}
      >
        {/* Savings input */}
        <Group gap={4}>
          <ThemeIcon color="yellow" variant="subtle">
            <IconCurrencyDollar size={16} />
          </ThemeIcon>
          <Text fw={700}>Savings</Text>
          <TextInput placeholder="Enter amount" size="sm" />
        </Group>

        {/* Bank Filter */}
        <Group gap={4}>
          <ThemeIcon color="green" variant="subtle">
            <IconRepeat size={16} />
          </ThemeIcon>
          <Text fw={700}>Cycle</Text>
          <MultiSelect
            // label="Your favorite libraries"
            placeholder="Pick value"
            data={["React", "Angular", "Vue", "Svelte"]}
            searchable
          />
        </Group>

        {/* Sort By */}
        <Group gap={4}>
          <ThemeIcon color="teal" variant="subtle">
            <IconCalendar size={16} />
          </ThemeIcon>
          <Text fw={700}>Year</Text>
          <Select
            size="sm"
            data={[
              { value: "TotalRate desc", label: "Total Rate" },
              { value: "BaseRate desc", label: "Base Rate" },
              { value: "BonusRate desc", label: "Bonus Rate" },
              { value: "Bank", label: "Bank" },
              { value: "Name", label: "Account Name" },
            ]}
            defaultValue="TotalRate desc"
            onChange={(value, option) => changeSort(value)}
          />
        </Group>

        {/* Toggles */}
        <Group gap="sm">
          <Group gap={4}>
            <ThemeIcon color="pink" variant="subtle">
              <IconPig size={16} />
            </ThemeIcon>
            <Text fw={700}>Super</Text>
            <Switch color="pink" defaultChecked />
          </Group>

          <Group gap={4}>
            <ThemeIcon color="yellow" variant="subtle">
              <IconSchool size={16} />
            </ThemeIcon>
            <Text fw={700}>HECS</Text>
            <Switch />
          </Group>

          <Group gap={4}>
            <ThemeIcon color="blue" variant="subtle">
              <IconHospital size={16} />
            </ThemeIcon>
            <Text fw={700}>Hospital</Text>
            <Switch />
          </Group>
        </Group>
      </Paper>

      <Stack>
        {savings.map((account, index) => {
          const isLast = index === savings.length - 1;
          return (
            <div key={account.id} ref={isLast ? lastElementRef : null}>
              <SavingsCard account={account} />
            </div>
          );
        })}
      </Stack>
    </Container>
  );
}
