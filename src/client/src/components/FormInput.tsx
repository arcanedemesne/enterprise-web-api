import Input from "@mui/joy/Input";
import FormControl from "@mui/joy/FormControl";
import FormLabel from "@mui/joy/FormLabel";
import FormHelperText from "@mui/joy/FormHelperText";

export interface IFormInputProps {
  label?: string | null;
  type: string;
  name: string;
  placeholder?: string | null;
  value: string;
  onChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onKeyUp?: (event: React.KeyboardEvent<HTMLInputElement>) => void;
  hidden?: boolean;
  disabled?: boolean;
  error?: string;
}

const FormInput = ({
  label,
  type,
  name,
  placeholder,
  value,
  onChange,
  onKeyUp,
  hidden,
  disabled,
  error,
}: IFormInputProps) => {
  return (
    <>
      <FormControl sx={{ mb: 2 }} error={!!error && error!.length > 0}>
        {label && <FormLabel>{label}</FormLabel>}
        <Input
          // html input attribute
          type={type}
          name={name}
          placeholder={placeholder || ""}
          value={value}
          onChange={onChange}
          onKeyUp={onKeyUp}
          sx={{
            display: hidden ? "none" : "auto",
          }}
          disabled={disabled}
          error={!!error && error!.length > 0}
        />
        <FormHelperText>{error}</FormHelperText>
      </FormControl>
    </>
  );
};

export default FormInput;
