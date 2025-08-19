import { Flex, Loader, Text } from '@mantine/core';

// 1. Define the props interface for type safety
interface LoadingProps {
    /** The text to display next to the loader. */
    text?: string;
    /** The size of the loader. Mantine's sizes are 'xs', 'sm', 'md', 'lg', 'xl'. */
    size?: 'xs' | 'sm' | 'md' | 'lg' | 'xl';
}


export function Loading({ text = 'Augnablik...', size = 'sm' }: LoadingProps) {
    return (
        <Flex
            gap="sm"
            justify="center"
            align="center"
        >
            <Loader size={size} />
            <Text>{text}</Text>
        </Flex>
    );
}