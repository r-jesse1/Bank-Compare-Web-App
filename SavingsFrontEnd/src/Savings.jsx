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
} from "@mantine/core";
import { useState, useEffect } from "react";

export function Savings() {
  const [savings, setSavings] = useState([]);

  useEffect(() => {
    fetchSavings();
  }, []);

  const fetchSavings = async () => {
    try {
      const response = await fetch("http://localhost:5134/savingsrate", {
        method: "GET",
      });
      const data = await response.json();

      setSavings(data);
      console.log(data);
    } catch (e) {
      console.log(e);
    }
  };

  return (
    <Container size="lg" mt="lg">
      <Title order={2} mb="md">
        Savings Accounts
      </Title>
      <Stack>
        {savings &&
          savings.map((account) => (
            <Card
              shadow="sm"
              radius="md"
              p="lg"
              withBorder
              style={{ maxWidth: 800, margin: "auto" }}
            >
              <Grid align="center">
                {/* Logo and Name */}
                <Grid.Col span={2}>
                  <Image
                    src={`https://logo.clearbit.com/${
                      new URL(account.url).hostname
                    }`}
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
                  <Button
                    component="a"
                    href={account.url}
                    target="_blank"
                    radius="xl"
                    fullWidth
                    color="dark"
                  >
                    Visit Site
                  </Button>
                </Grid.Col>
              </Grid>
            </Card>
          ))}
      </Stack>
    </Container>
  );
}
