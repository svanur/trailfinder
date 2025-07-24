// TrailFinder.Website\src\pages\AboutUs.tsx
import {Avatar, Container, Divider, Group, Paper, Text, Timeline, Title} from '@mantine/core';

import {IconGitPullRequest, IconRocket } from '@tabler/icons-react';
import {useEffect} from "react";

function AboutUsPage() {
    const nextUpdate =
        {
            description: 'Hlaupa Laugaveginn á sub 4 #teamSub4',
            icon: <IconGitPullRequest size={18}/>
        };

    const updateHistory = [
        {
            date: 'July 25, 2025',
            description: 'Hlaupaleiðir fer í loftið :D #GAMAN',
            icon: <IconRocket size={18}/>,
        },
    ];

    // Sort updateHistory: Newest first (descending order by date)
    const sortedUpdates = [...updateHistory].sort((a, b) =>
        new Date(b.date).getTime() - new Date(a.date).getTime()
    );

    // Use useEffect to run the obfuscation code after the component mounts
    useEffect(() => {
        const user = "svanur.karlsson";
        const domain = "gmail";
        const tld = "com";
        const emailLinkElement = document.getElementById("emailLink");

        if (emailLinkElement) {
            emailLinkElement.setAttribute("href", `mailto:${user}@${domain}.${tld}`);
            emailLinkElement.textContent = `${user} [hjá] ${domain} [punktur] ${tld}`;
        }
    }, []);

    return (
        <Container size="md" my="xl">
            <Paper shadow="sm" p="xl" withBorder>
                <Group align="center" gap="lg" justify="center">
                    <Avatar
                        src="/um.jpg"
                        alt="Svanur Karlsson"
                        size={120}
                        radius="50%"
                    />
                    <Title order={1} ta="center">
                        hlaupaleidir.is
                    </Title>
                    <Text size="lg" ta="center">
                        <Text>
                            Velkomin á Hlaupaleiðir.is. Hér sameinast nördisminn minn sem forritari og utanvegalaupari.
                            Ég hef í nokurn tíma safnað áhugaverðum hlaupaleiðum sem mig langar að fara 'einn góðan veðurdag'.
                        </Text>
                        <Text>
                            En ...svo er bara svo mikið annað að gera!
                            Vonandi kemst maður samt í að hlaupa eitthvað af öllum þessum leiðum í framtíðinni.
                        </Text>
                        <Text>Vonandi hafið þið gagn og gaman af og sjáumst á hlaupum :)</Text>
                    </Text>
                    <Text
                        ta="center"
                        style={{
                            fontFamily: "'Dancing Script', cursive",
                            fontWeight: 700,
                            lineHeight: 1,
                            width: '100%',
                            fontSize: '42px', // Custom font size - adjust this value as needed!
                        }}
                    >
                        <Text>kveðja,</Text>
                        Svanur Þór Karlsson
                    </Text>
                </Group>

                <Divider my="xl"/>

                <Title order={2} ta="center" mb="lg">
                    Markmiðið
                </Title>
                <Text ta="center">
                    Að hlauparar og útivistarfólk noti þennan vef til að finna og deila skemmtilegum leiðum út um
                    allt land.
                </Text>

                <Divider my="xl"/>

                <Title order={2} ta="center" mb="lg">
                    Uppfærslur
                </Title>
                <Timeline active={sortedUpdates.length} bulletSize={24} lineWidth={2}>
                    <Timeline.Item
                        bullet={nextUpdate.icon}
                        title={<Text c="dimmed" fs="italic">Næsta viðbót</Text>}
                        lineVariant="dotted"
                        color="gray"
                        p="md"
                    >
                        <Text c="dimmed" size="sm" fs="italic">
                            {nextUpdate.description}
                        </Text>
                    </Timeline.Item>

                    {sortedUpdates.map((update, index) => (
                        <Timeline.Item
                            key={index}
                            bullet={update.icon}
                            title={update.date}
                        >
                            <Text c="dimmed" size="sm">
                                {update.description}
                            </Text>
                        </Timeline.Item>
                    ))}
                </Timeline>

                <Divider my="xl"/>

                <Title c="dimmed" order={2} ta="center" mb="lg">
                    Ert þú með hugmynd?
                </Title>
                <Text c="dimmed" ta="center">
                    Sendu mér þína frábæru hugmynd fyrir vefinn á netfangið {' '}
                    <a id="emailLink" style={{textDecoration: 'none', color: 'inherit'}}>

                    </a>
                </Text>
                <Text c="dimmed" ta="center">...og hver veit nema hún komist einn daginn á uppfærslulistann :)</Text>

            </Paper>
        </Container>
    )
}

export default AboutUsPage;