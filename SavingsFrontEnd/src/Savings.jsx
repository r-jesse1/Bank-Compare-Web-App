import {
  Text,
  Stack,
  Container,
  Title,
  Group,
  Paper,
  Select,
  Switch,
  ThemeIcon,
  MultiSelect,
  NumberInput,
  Loader,
} from "@mantine/core";

import {
  IconCurrencyDollar,
  IconPig,
  IconBuildingBank,
  IconSortDescending,
  IconGift,
} from "@tabler/icons-react";

import {
  useMantineColorScheme,
  ActionIcon,
  useComputedColorScheme,
} from "@mantine/core";
import "./App.css";

import { bankList } from "./BankList";

import { IconSun, IconMoon } from "@tabler/icons-react";
import { SavingsCard } from "./SavingsCard";

import { useEffect, useState, useRef, useCallback } from "react";

export function Savings() {
  const [savings, setSavings] = useState([]);
  const [userBalance, setUserBalance] = useState(0);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [sortBy, setSortBy] = useState("TotalRate desc");
  const [selectedBanks, setSelectedBanks] = useState([]);

  const [bonus, setBonus] = useState(true);
  const [intro, setIntro] = useState(true);

  const [error, setError] = useState(null);
  const observer = useRef();

  const { setColorScheme } = useMantineColorScheme();
  const computedColorScheme = useComputedColorScheme("light", {
    getInitialValueInEffect: true,
  });

  function getRateType() {
    if (bonus && intro) {
      return "all";
    }
    if (!bonus && !intro) {
      return "none";
    }
    if (bonus) {
      return "bonus";
    }
    if (intro) {
      return "intro";
    }
    return "all";
  }

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
      const rateType = getRateType();
      const params = new URLSearchParams({
        pageNumber: pageNumber.toString(),
        pageSize: "10",
        sortBy: sortBy,
        rateType: rateType,
      });

      selectedBanks.forEach((bank) => params.append("banks", bank));

      const response = await fetch(
        `http://localhost:5134/savingsrate?${params.toString()}`
      );

      if (!response.ok) {
        throw new Error(`Server error: ${response.status}`);
      }

      const data = await response.json();

      if (data.length === 0) {
        setHasMore(false);
      } else {
        // Prevent duplicates by filtering based on account.id
        setSavings((prev) => {
          const existingIds = new Set(prev.map((a) => a.id));
          const newUnique = data.filter((a) => !existingIds.has(a.id));
          setLoading(false);
          setError(null);
          return [...prev, ...newUnique];
        });
      }
    } catch (e) {
      console.log(e);
      setError(e.message || "An Unexpected Error Occurred");
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSavings(page);
  }, [page, sortBy, selectedBanks, bonus, intro]);

  function resetList() {
    setLoading(true);
    setSavings([]);
    setPage(1);
  }

  console.log(savings.map((a) => a));

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
          <NumberInput
            placeholder="Enter amount"
            size="sm"
            hideControls
            onChange={(val) => setUserBalance(val)}
          />
        </Group>

        {/* Bank Filter */}
        <Group gap={4}>
          <ThemeIcon color="green" variant="subtle">
            <IconBuildingBank size={16} />
          </ThemeIcon>
          <Text fw={700}>Filter Banks</Text>
          <MultiSelect
            // label="Your favorite libraries"
            placeholder="Filter Banks"
            data={bankList}
            searchable
            clearable
            onChange={(value) => {
              resetList();
              setSelectedBanks(value);
            }}
          />
        </Group>

        {/* Sort By */}
        <Group gap={4}>
          <ThemeIcon color="teal" variant="subtle">
            <IconSortDescending size={16} />
          </ThemeIcon>
          <Text fw={700}>Sort</Text>
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
            onChange={(value, option) => {
              resetList();
              setSortBy(value);
            }}
          />
        </Group>

        {/* Toggles */}
        <Group gap="sm">
          <Group gap={4}>
            <ThemeIcon color="pink" variant="subtle">
              <IconGift size={16} />
            </ThemeIcon>
            <Text fw={700}>Bonus Accounts</Text>
            <Switch
              color="pink"
              defaultChecked
              checked={bonus}
              onChange={(event) => {
                resetList();
                setBonus(event.currentTarget.checked);
              }}
            />
          </Group>

          <Group gap={4}>
            <ThemeIcon color="yellow" variant="subtle">
              <IconPig size={16} />
            </ThemeIcon>
            <Text fw={700}>Intro Accounts</Text>
            <Switch
              color="yellow"
              defaultChecked
              checked={intro}
              onChange={(event) => {
                resetList();
                setIntro(event.currentTarget.checked);
              }}
            />
          </Group>
        </Group>
      </Paper>
      {loading ? (
        <div
          style={{
            height: "78vh",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
          }}
        >
          <Loader color="gray" size="xl" type="bars" />
        </div>
      ) : error ? (
        <div
          style={{
            height: "78vh",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            color: "red",
            textAlign: "center",
            padding: "1rem",
          }}
        >
          <p>{error}</p>
        </div>
      ) : (
        <Stack>
          {savings.map((account, index) => {
            const isLast = index === savings.length - 1;
            return (
              <div key={account.id} ref={isLast ? lastElementRef : null}>
                <SavingsCard account={account} userBalance={userBalance} />
              </div>
            );
          })}
        </Stack>
      )}
    </Container>
  );
}
