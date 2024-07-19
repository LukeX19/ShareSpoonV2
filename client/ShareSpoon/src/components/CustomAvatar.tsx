import React from 'react';
import { Avatar, SxProps, Theme } from '@mui/material';

interface CustomAvatarProps {
  firstName: string;
  lastName: string;
  pictureURL: string;
  sx?: SxProps<Theme>;
}

const getInitials = (firstName: string, lastName: string): string => {
  return `${firstName.charAt(0)}${lastName.charAt(0)}`;
};

const CustomAvatar: React.FC<CustomAvatarProps> = ({ firstName, lastName, pictureURL, sx }) => {
  return pictureURL !== "default" ? (
    <Avatar alt={`${firstName} ${lastName}`} src={pictureURL} sx={sx} />
  ) : (
    <Avatar sx={sx}>{getInitials(firstName, lastName)}</Avatar>
  );
};

export default CustomAvatar;
