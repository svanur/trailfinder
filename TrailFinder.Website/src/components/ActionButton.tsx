import React, {type SVGAttributes } from 'react';
import { ActionIcon } from '@mantine/core';

interface ActionButtonProps {
    icon: React.ReactElement<SVGAttributes<SVGElement>>;
    label: string;
    onClick: () => void;
    variant?: 'default' | 'filled';
    disabled?: boolean;
}

const ActionButton: React.FC<ActionButtonProps> = ({
                                                       icon,
                                                       label,
                                                       onClick,
                                                       variant = 'default',
                                                       disabled = false
                                                   }) => {
    return (
        <ActionIcon
            variant={variant}
            size="lg"
            onClick={onClick}
            disabled={disabled}
            aria-label={label}
        >
            {React.cloneElement<SVGAttributes<SVGElement>>(icon, {
                width: '70%',
                height: '70%',
                strokeWidth: 1.5
            })}
        </ActionIcon>
    );
};

export default ActionButton;