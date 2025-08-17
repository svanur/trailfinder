// src/components/TrailLoader.tsx

import { Container, Text, Stack } from '@mantine/core';
import { IconRun } from '@tabler/icons-react';
import { useState, useEffect } from 'react';

// You might put this CSS in a global stylesheet or a separate CSS module
const runAnimation = `
  @keyframes run-on-spot {
    0% {
      transform: translateX(0);
    }
    25% {
      transform: translateX(2px) rotate(1deg);
    }
    50% {
      transform: translateX(0);
    }
    75% {
      transform: translateX(-2px) rotate(-1deg);
    }
    100% {
      transform: translateX(0);
    }
  }
`;

export function TrailLoader() {
    // Array of messages to rotate through
    const warmUpMessages = [
        "Sveifla fótum", // Swing legs
        "Sveifla höndum", // Swing arms
        "Teygja smá",    // Stretch a little
        "Draga djúpt andann", // Breathe deeply
        "Binda skóreimar",   // Tie shoelaces
        "Rólegt skokk",
    ];

    // State to keep track of the current message index
    const [messageIndex, setMessageIndex] = useState(0);

    useEffect(() => {
        // Set up an interval to change the message every 2 seconds
        const interval = setInterval(() => {
            setMessageIndex((prevIndex) => (prevIndex + 1) % warmUpMessages.length);
        }, 2000); // Change message every 2000 milliseconds (2 seconds)

        // Clear the interval when the component unmounts to prevent memory leaks
        return () => clearInterval(interval);
    }, [warmUpMessages.length]); // Re-run effect if messages array length changes

    return (
        <>
            {/* Inject the keyframes into the DOM. For larger projects, use a CSS file. */}
            <style>{runAnimation}</style>
            <Container ta="center" style={{ padding: '4rem 0' }}>
                <Stack align="center" gap="md">
                    <IconRun
                        size={80}
                        stroke={1.5}
                        color="var(--mantine-color-blue-6)"
                        style={{
                            animation: 'run-on-spot 1s infinite ease-in-out',
                        }}
                    />
                    <Text size="xl" fw={700} c="dimmed">
                        Við skulum hita upp á meðan við finnum slóðina fyrir þig!
                    </Text>
                    <Text size="xl" fw={700} c="dimmed">
                        {warmUpMessages[messageIndex]}
                    </Text>
                </Stack>
            </Container>
        </>
    );
}
