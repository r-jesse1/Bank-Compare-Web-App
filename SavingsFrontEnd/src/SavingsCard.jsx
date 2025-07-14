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
} from "@mantine/core";
import {
  MantineProvider,
  useMantineColorScheme,
  ActionIcon,
  useComputedColorScheme,
} from "@mantine/core";
import { IconSun, IconMoon } from "@tabler/icons-react";

import { useState, useEffect } from "react";

export function SavingsCard({ account }) {
  return (
    <Card style={{ maxWidth: 800, margin: "auto" }}>
      <Grid align="center">
        {/* Logo and Name */}
        <Grid.Col span={2}>
          <Image
            src={`https://logo.clearbit.com/${new URL(account.url).hostname}`}
            alt="Bank logo"
            width={60}
            height={60}
            fit="contain"
          />
        </Grid.Col>

        <Grid.Col span={6}>
          <Text size="xl" fw={700}>
            {account.bank} - {account.name}
          </Text>
        </Grid.Col>

        {/* Interest Breakdown */}
        <Grid.Col span={4}>
          <Group position="apart">
            <Stack spacing={2} align="center">
              <Text fw={600}>{account.baseRate?.toFixed(2)}%</Text>
              <Text size="sm" c="dimmed">
                Base Rate
              </Text>
            </Stack>
            <Stack spacing={2} align="center">
              <Text fw={600}>{account.bonusRate?.toFixed(2)}%</Text>
              <Text size="sm" c="dimmed">
                Bonus Rate
              </Text>
            </Stack>
            <Stack spacing={2} align="center">
              <Text fw={600}>{account.totalRate?.toFixed(2)}%</Text>
              <Text size="sm" c="dimmed">
                Total Rate
              </Text>
            </Stack>
          </Group>
        </Grid.Col>

        {/* Description and CTA */}
        <Grid.Col span={8}>
          <Text size="sm" mt="sm">
            {account.bonusConditions || "No bonus conditions provided."}
          </Text>
        </Grid.Col>

        {/* CTA Button */}
        <Grid.Col span={2} offset={10} mt="md">
          <Button component="a" href={account.url} target="_blank">
            Visit Site
          </Button>
        </Grid.Col>
      </Grid>
    </Card>
  );
}
