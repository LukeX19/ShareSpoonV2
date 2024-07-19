import authAxios from "../config/authConfig";

export const getUsersActivity = async (daysNumber: number, pageIndex: number, pageSize: number = 10) => {
  const response = await authAxios.get('/api/users/activity', {
    params: {
      daysNumber: daysNumber,
      PageIndex: pageIndex,
      PageSize: pageSize
    }
  });
  return response.data;
};

export const getCurrentUser = async () => {
  const response = await authAxios.get('/api/users/current');
  return response;
};

export const updateUserRole = async (userId: string, role: number) => {
  const response = await authAxios.put('/api/users/changeRole', {
    userId: userId,
    role: role
  });
  return response;
};
