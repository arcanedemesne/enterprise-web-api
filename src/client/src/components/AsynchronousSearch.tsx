import * as React from 'react';
import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import Autocomplete from '@mui/joy/Autocomplete';
import CircularProgress from '@mui/joy/CircularProgress';

export interface IOption {
  label: string;
  id: any;
}

interface AsynchronousSearchProps {
  label: string;
  options: IOption[];
  value?: IOption | undefined;
  handleChange: (event: any, option: IOption) => void;
  inputValue: string | undefined;
  handleInputChange: (event: any, label: string) => void;
}

const AsynchronousSearch = ({ label, options, value, handleChange, inputValue, handleInputChange }: AsynchronousSearchProps) => {
  const [open, setOpen] = React.useState(false);
  const loading = open && options.length === 0;

  return (
    <FormControl id="asynchronous-search">
      <FormLabel>{label}</FormLabel>
      <Autocomplete
        sx={{ width: 300 }}
        placeholder="Asynchronous"
        open={open}
        onOpen={() => {
          setOpen(true);
        }}
        onClose={() => {
          setOpen(false);
        }}
        value={value}
        onChange={handleChange}
        inputValue={inputValue}
        onInputChange={handleInputChange}
        isOptionEqualToValue={(option: any, value: any) => option === value || option.id === value.id}
        getOptionLabel={(option) => option.label}
        disableClearable
        options={options}
        loading={loading}
        endDecorator={
          loading ? (
            <CircularProgress size="sm" sx={{ bgcolor: 'background.surface' }} />
          ) : null
        }
      />
    </FormControl>
  );
}

export default AsynchronousSearch;
