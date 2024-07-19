import React, { useEffect, useState } from 'react';
import { DataGrid, GridColDef, GridRenderCellParams } from '@mui/x-data-grid';
import { getUsersActivity, updateUserRole } from '../services/UserService';
import { Helmet } from 'react-helmet-async';
import { Button, FormControl, Select, MenuItem, Box } from '@mui/material';
import { toast } from 'react-toastify';

const AdminPanel: React.FC = () => {
  const [daysNumber, setDaysNumber] = useState(7);
  const [users, setUsers] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [paginationModel, setPaginationModel] = useState<{ page: number; pageSize: number }>({
    page: 0,
    pageSize: 10,
  });
  const [rowCount, setRowCount] = useState<number>(0);

  const handleActionClick = async (row: any) => {
    const newRole = row.role === 2 ? 1 : 2;
    try {
      const response = await updateUserRole(row.id, newRole);
      if(response && response.status === 200) {
        newRole === 2 ? toast.success("User promoted successfully!") : toast.success("User demoted successfully!");
        setUsers((prevUsers) =>
        prevUsers.map((user) =>
          user.id === row.id ? { ...user, role: newRole } : user
        ));
      }
    } catch (error) {
      console.log(error);
      toast.error("An error has occured! Please try again later.");
    }
  };

  const columns: GridColDef[] = [
    {
      field: 'firstName',
      headerName: 'First name',
      flex: 1,
      editable: false,
      align: 'center',
      headerAlign: 'center'
    },
    {
      field: 'lastName',
      headerName: 'Last name',
      flex: 1,
      editable: false,
      align: 'center',
      headerAlign: 'center'
    },
    {
      field: 'email',
      headerName: 'Email',
      flex: 1.5,
      editable: false,
      align: 'center',
      headerAlign: 'center'
    },
    {
      field: 'age',
      headerName: 'Age',
      type: 'number',
      flex: 0.5,
      editable: false,
      align: 'center',
      headerAlign: 'center'
    },
    {
      field: 'postedRecipesCounter',
      headerName: 'Posted Recipes',
      type: 'number',
      flex: 1,
      editable: false,
      align: 'center',
      headerAlign: 'center'
    },
    {
      field: 'receivedLikesCounter',
      headerName: 'Received Likes',
      type: 'number',
      flex: 1,
      editable: false,
      align: 'center',
      headerAlign: 'center'
    },
    {
      field: 'action',
      headerName: 'Action',
      flex: 1,
      sortable: false,
      align: 'center',
      headerAlign: 'center',
      renderCell: (params: GridRenderCellParams) => {
        const role = params.row.role;
        const isPromoted = role === 2;
        return (
          <Button
            variant="contained"
            color={isPromoted ? "error" : "success"}
            onClick={() => handleActionClick(params.row)}
            sx={{width: '100%'}}
          >
            {isPromoted ? "Demote" : "Promote"}
          </Button>
        );
      },
    },
  ];

  useEffect(() => {
    const fetchUsersActivity = async () => {
      try {
        setLoading(true);
        const response = await getUsersActivity(daysNumber, paginationModel.page + 1, paginationModel.pageSize);
        if (response) {
          setUsers(response.elements);
          setRowCount(response.resultsCount);
        }
      } catch (error) {
        console.log(error);
      } finally {
        setLoading(false);
      }
    };
    fetchUsersActivity();
  }, [paginationModel, daysNumber]);

  const handlePaginationModelChange = (model: { page: number; pageSize: number }) => {
    setPaginationModel(model);
  };

  return (
    <>
      <Helmet>
        <title>Admin Panel</title>
      </Helmet>
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'flex-start',
          minHeight: '100vh',
          width: '100%',
        }}
      >
        <Box
          sx={{
            width: '100%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            mb: 4,
            mt: 14,
            px: 2
          }}
        >
          Users Activity in the Last 
          <Box sx={{ minWidth: 120, ml: 1 }}>
      <FormControl fullWidth>
        <Select
          defaultValue={daysNumber}
          value={daysNumber}
          name='daysNumber'
          size='small'
          onChange={(event) => setDaysNumber(Number(event.target.value))}
        >
          <MenuItem value={7}>7 Days</MenuItem>
          <MenuItem value={14}>14 Days</MenuItem>
          <MenuItem value={30}>30 Days</MenuItem>
        </Select>
      </FormControl>
    </Box>
        </Box>
        <Box sx={{ width: '100%', maxWidth: 1000, minHeight: 829, pl: 2 }}>
          <DataGrid
            rows={users}
            columns={columns}
            pagination
            paginationMode="server"
            rowCount={rowCount}
            paginationModel={paginationModel}
            onPaginationModelChange={handlePaginationModelChange}
            pageSizeOptions={[5, 10, 20]}
            loading={loading}
            disableRowSelectionOnClick
          />
        </Box>
      </Box>
    </>
  );
};

export default AdminPanel;
